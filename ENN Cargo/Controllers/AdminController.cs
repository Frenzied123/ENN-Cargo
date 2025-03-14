using ENN_Cargo.Core;
using ENN_Cargo.DataAccess;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ENN_Cargo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ENN_CargoApplicationDbContext _context;
        private readonly IDriverService _driverService;
        private readonly ITruckCompanyService _truckCompanyService;
        private readonly ICompanyStockService _companyStockService;
        private readonly IShipmentService _shipmentService;
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<IdentityUser> userManager,
            ENN_CargoApplicationDbContext context,
            IDriverService driverService,
            ITruckCompanyService truckCompanyService,
            ICompanyStockService companyStockService,
            IShipmentService shipmentService,
            IVehicleService vehicleService,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _context = context;
            _driverService = driverService;
            _truckCompanyService = truckCompanyService;
            _companyStockService = companyStockService;
            _shipmentService = shipmentService;
            _vehicleService = vehicleService;
            _logger = logger;
        }

        public IActionResult AdminPage()
        {
            var rawRequests = _context.PendingRequests
                .Where(r => r.Status == "Pending")
                .OrderBy(r => r.CreatedAt)
                .ToList();

            var requests = rawRequests.Select(r => new PendingRequestViewModel
            {
                Id = r.Id,
                Type = r.Type.Split(':')[0],
                Request = r.Type.Split(':').Length > 1 ? r.Type.Split(':')[1] : r.Type,
                Details = "Details packed in Request",
                CreatedAt = r.CreatedAt,
                Status = r.Status,
                UserId = r.UserId
            }).ToList();

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.PendingRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null || request.Status != "Pending")
            {
                TempData["Error"] = "Request not found or already processed.";
                return RedirectToAction("AdminPage");
            }

            try
            {
                request.Status = "Approved";
                await ProcessRequest(request);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Request {request.Type.Split(':')[0]} approved successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving request {Id}", id);
                request.Status = "Pending";
                TempData["Error"] = "Failed to approve request.";
            }
            return RedirectToAction("AdminPage");
        }

        [HttpPost]
        public async Task<IActionResult> Decline(int id)
        {
            var request = await _context.PendingRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null || request.Status != "Pending")
            {
                TempData["Error"] = "Request not found or already processed.";
                return RedirectToAction("AdminPage");
            }

            try
            {
                var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.PendingRequest_Id == id);
                if (driver != null) _context.Drivers.Remove(driver);
                var truckCompany = await _context.TruckCompanies.FirstOrDefaultAsync(tc => tc.PendingRequest_Id == id);
                if (truckCompany != null) _context.TruckCompanies.Remove(truckCompany);
                var companyStock = await _context.CompanyStocks.FirstOrDefaultAsync(cs => cs.PendingRequest_Id == id);
                if (companyStock != null) _context.CompanyStocks.Remove(companyStock);
                var shipment = await _context.Shipments.FirstOrDefaultAsync(s => s.PendingRequest_Id == id);
                if (shipment != null) _context.Shipments.Remove(shipment);
                var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.PendingRequest_Id == id);
                if (vehicle != null) _context.Vehicles.Remove(vehicle);
                _context.PendingRequests.Remove(request);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Request {request.Type.Split(':')[0]} declined successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error declining request {Id}", id);
                TempData["Error"] = "Failed to decline request.";
            }
            return RedirectToAction("AdminPage");
        }

        private async Task ProcessRequest(PendingRequest request)
        {
            var parts = request.Type.Split(new[] { ':' }, 2);
            if (parts.Length < 2)
                throw new Exception("Invalid request data.");

            var type = parts[0];
            var detailsString = parts[1];
            var details = detailsString.Split(", ")
                .Select(kv => kv.Split(new[] { '=' }, 2))
                .Where(kv => kv.Length == 2)
                .ToDictionary(
                    kv => kv[0].Trim(),
                    kv => kv[1].Trim(),
                    StringComparer.OrdinalIgnoreCase
                );

            if (type == "DriverRegistration" || type == "TruckCompanyRegistration" || type == "ShipmentCompanyRegistration")
            {
                if (!details.ContainsKey("Email") || !details.ContainsKey("Password"))
                    throw new Exception("Missing required user fields.");

                var user = new IdentityUser
                {
                    UserName = details["Email"],
                    Email = details["Email"],
                    PhoneNumber = details.ContainsKey("Phone") ? details["Phone"] : null
                };
                var result = await _userManager.CreateAsync(user, details["Password"]);
                if (!result.Succeeded)
                    throw new Exception("Failed to create user.");
            }

            switch (type)
            {
                case "DriverRegistration":
                    await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(details["Email"]), "Driver");
                    if (!details.ContainsKey("Name") || !details.ContainsKey("Experience"))
                        throw new Exception("Missing required driver fields.");
                    var nameParts = details["Name"].Split(' ', 2);
                    var driver = new Driver
                    {
                        FirstName = nameParts[0],
                        LastName = nameParts.Length > 1 ? nameParts[1] : "",
                        Experience = int.Parse(details["Experience"]),
                        UserId = (await _userManager.FindByEmailAsync(details["Email"])).Id,
                        PendingRequest_Id = request.Id
                    };
                    _context.Drivers.Add(driver);
                    break;

                case "TruckCompanyRegistration":
                    await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(details["Email"]), "TruckCompany");
                    if (!details.ContainsKey("Name") || !details.ContainsKey("Address") || !details.ContainsKey("Country") || !details.ContainsKey("Town"))
                        throw new Exception("Missing required truck company fields.");
                    var truckCompany = new TruckCompany
                    {
                        Name = details["Name"],
                        Address = details["Address"],
                        Country = details["Country"],
                        Town = details["Town"],
                        UserId = (await _userManager.FindByEmailAsync(details["Email"])).Id,
                        PendingRequest_Id = request.Id
                    };
                    _context.TruckCompanies.Add(truckCompany);
                    break;

                case "ShipmentCompanyRegistration":
                    await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(details["Email"]), "ShipmentCompany");
                    if (!details.ContainsKey("Name") || !details.ContainsKey("Address") || !details.ContainsKey("Country") || !details.ContainsKey("Town"))
                        throw new Exception("Missing required shipment company fields.");
                    var companyStock = new CompanyStock
                    {
                        Name = details["Name"],
                        Address = details["Address"],
                        Country = details["Country"],
                        Town = details["Town"],
                        UserId = (await _userManager.FindByEmailAsync(details["Email"])).Id,
                        PendingRequest_Id = request.Id
                    };
                    _context.CompanyStocks.Add(companyStock);
                    break;

                case "ShipmentCreation":
                    if (!details.ContainsKey("Description") || !details.ContainsKey("Weight") || !details.ContainsKey("From") || !details.ContainsKey("to") || !details.ContainsKey("CompanyStockId"))
                        throw new Exception("Missing required shipment fields.");
                    var shipment = new Shipment
                    {
                        Description = details["Description"],
                        Weight = double.Parse(details["Weight"]),
                        FromAddress = details["FromAddress"],
                        FromCountry = details["FromCountry"],
                        FromTown = details["From"],
                        ToAddress = details["ToAddress"],
                        ToCountry = details["ToCountry"],
                        ToTown = details["to"],
                        PickUpDate = details.ContainsKey("Pickup") ? DateTime.Parse(details["Pickup"]) : null,
                        DeliveryDate = details.ContainsKey("Delivery") ? DateTime.Parse(details["Delivery"]) : null,
                        Status = "Pending",
                        PendingRequest_Id = request.Id
                    };
                    _context.Shipments.Add(shipment);
                    _context.CompanyStocks_Shipments.Add(new CompanyStocks_Shipments
                    {
                        Shipment = shipment,
                        CompanyStock_Id = int.Parse(details["CompanyStockId"])
                    });
                    break;

                case "VehicleCreation":
                    if (!details.ContainsKey("LicensePlate") || !details.ContainsKey("TruckCompanyId") || !details.ContainsKey("Brand") || !details.ContainsKey("Model") || !details.ContainsKey("Year"))
                        throw new Exception("Missing required vehicle fields.");
                    var vehicle = new Vehicle
                    {
                        LicensePlate = details["LicensePlate"],
                        TruckCompany_Id = int.Parse(details["TruckCompanyId"]),
                        Brand = details["Brand"],
                        Model = details["Model"],
                        Year = int.Parse(details["Year"]),
                        PendingRequest_Id = request.Id
                    };
                    _context.Vehicles.Add(vehicle);
                    break;

                default:
                    throw new Exception("Unknown request type.");
            }
        }
    }
}
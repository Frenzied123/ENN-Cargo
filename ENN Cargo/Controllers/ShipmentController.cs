using ENN_Cargo.Core;
using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;[Authorize]
public class ShipmentController : Controller
{
    private readonly IShipmentService _shipmentService;
    private readonly ICompanyStockService _companyStockService;
    private readonly ITruckCompanyService _truckCompanyService;   
    private readonly IDriverService _driverService;               
    private readonly IVehicleService _vehicleService;          
    private readonly IRepository<CompanyStock> _companyStockRepository;
    private readonly IRepository<CompanyStocks_Shipments> _companyStocksShipmentsRepository;
    private readonly UserManager<IdentityUser> _userManager;    private static readonly Dictionary<string, List<string>> PredefinedTowns = new()
    {
        { "Bulgaria", new() { "Sofia", "Plovdiv", "Varna" } },
        { "Turkey", new() { "Istanbul", "Ankara", "Izmir" } },
        { "Romania", new() { "Bucharest", "Cluj-Napoca", "Timisoara" } }
    };
    private static readonly List<string> PredefinedCountries = new() { "Bulgaria", "Turkey", "Romania" };    public ShipmentController(
        IShipmentService shipmentService,
        ICompanyStockService companyStockService,
        ITruckCompanyService truckCompanyService,      
        IDriverService driverService,               
        IVehicleService vehicleService,             
        IRepository<CompanyStock> companyStockRepository,
        IRepository<CompanyStocks_Shipments> companyStocksShipmentsRepository,
        UserManager<IdentityUser> userManager)
    {
        _shipmentService = shipmentService;
        _companyStockService = companyStockService;
        _truckCompanyService = truckCompanyService;       
        _driverService = driverService;                
        _vehicleService = vehicleService;              
        _companyStockRepository = companyStockRepository;
        _companyStocksShipmentsRepository = companyStocksShipmentsRepository;
        _userManager = userManager;
    }    [HttpGet]
    public async Task<IActionResult> ListOfShipments(ShipmentViewModel model)
    {
        var shipments = (await _shipmentService.GetAllAsync()).ToList();
        shipments = ApplyFilters(model, shipments);
        model.Shipments = shipments;
        model.FromCountries = new SelectList(PredefinedCountries, model.SelectedFromCountry);
        model.ToCountries = new SelectList(PredefinedCountries, model.SelectedToCountry);
        model.FromTowns = new SelectList(
            !string.IsNullOrEmpty(model.SelectedFromCountry) && PredefinedTowns.ContainsKey(model.SelectedFromCountry)
                ? PredefinedTowns[model.SelectedFromCountry]
                : Enumerable.Empty<string>(),
            model.SelectedFromTown
        );
        model.ToTowns = new SelectList(
            !string.IsNullOrEmpty(model.SelectedToCountry) && PredefinedTowns.ContainsKey(model.SelectedToCountry)
                ? PredefinedTowns[model.SelectedToCountry]
                : Enumerable.Empty<string>(),
            model.SelectedToTown
        );
        model.PredefinedTowns = PredefinedTowns;        return View(model);
    }    private static List<Shipment> ApplyFilters(ShipmentViewModel model, List<Shipment> shipments)
    {
        if (model.MinWeight.HasValue) shipments = shipments.Where(s => s.Weight >= model.MinWeight).ToList();
        if (model.MaxWeight.HasValue) shipments = shipments.Where(s => s.Weight <= model.MaxWeight).ToList();
        if (!string.IsNullOrEmpty(model.SelectedFromCountry)) shipments = shipments.Where(s => s.FromCountry == model.SelectedFromCountry).ToList();
        if (!string.IsNullOrEmpty(model.SelectedFromTown)) shipments = shipments.Where(s => s.FromTown == model.SelectedFromTown).ToList();
        if (!string.IsNullOrEmpty(model.SelectedToCountry)) shipments = shipments.Where(s => s.ToCountry == model.SelectedToCountry).ToList();
        if (!string.IsNullOrEmpty(model.SelectedToTown)) shipments = shipments.Where(s => s.ToTown == model.SelectedToTown).ToList();
        if (model.PickUpDate.HasValue) shipments = shipments.Where(s => s.PickUpDate >= model.PickUpDate).ToList();
        if (model.DeliveryDate.HasValue) shipments = shipments.Where(s => s.DeliveryDate >= model.DeliveryDate).ToList();
        return shipments;
    }    [HttpGet]
    [Authorize(Roles = "Admin,ShipmentCompany")]
    public async Task<IActionResult> AddShipment()
    {
        var model = await PopulateDropdowns(new ShipmentViewModel());
        return View(model);
    }    [HttpPost]
    [Authorize(Roles = "Admin,ShipmentCompany")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddShipment(ShipmentViewModel model)
    {
        if (!string.IsNullOrEmpty(model.Description) &&
            model.Weight != 0 &&
            !string.IsNullOrEmpty(model.SelectedFromCountry) &&
            !string.IsNullOrEmpty(model.SelectedFromTown) &&
            !string.IsNullOrEmpty(model.FromAddress) &&
            !string.IsNullOrEmpty(model.SelectedToCountry) &&
            !string.IsNullOrEmpty(model.SelectedToTown) &&
            !string.IsNullOrEmpty(model.ToAddress) &&
            model.PickUpDate.HasValue &&
            model.DeliveryDate.HasValue)
        {
            var shipment = new Shipment
            {
                Description = model.Description,
                Weight = model.Weight,
                FromCountry = model.SelectedFromCountry,
                FromTown = model.SelectedFromTown,
                FromAddress = model.FromAddress,
                ToCountry = model.SelectedToCountry,
                ToTown = model.SelectedToTown,
                ToAddress = model.ToAddress,
                PickUpDate = model.PickUpDate.Value,
                DeliveryDate = model.DeliveryDate.Value,
                Status = "Available"             };
            await _shipmentService.AddAsync(shipment, model.SelectedCompanyStockId.GetValueOrDefault());
            return RedirectToAction("ListOfShipments");
        }
        model = await PopulateDropdowns(model);
        return View(model);
    }    [HttpGet]
    [Authorize(Roles = "TruckCompany,Admin")]
    public async Task<IActionResult> AssignShipment(int id)
    {
        var shipment = await _shipmentService.GetByIdAsync(id);
            if (shipment.Status != "Available")
            {
                return NotFound($"Shipment with ID {id} is not Available.");
            }            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not authenticated.");
            }            var allTruckCompanies = await _truckCompanyService.GetAllAsync();
            var currentTruckCompany = allTruckCompanies.FirstOrDefault(tc => tc.UserId == user.Id);
            if (!User.IsInRole("Admin") && currentTruckCompany == null)
            {
                return Unauthorized("No truck company associated with this user.");
            }            var allDrivers = await _driverService.GetAllAsync();            var allVehicles = await _vehicleService.GetAllAsync();            var model = new AssignShipmentViewModel
            {
                ShipmentId = id,
                DriverOptions = User.IsInRole("Admin")
                    ? allDrivers.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = $"{d.FirstName} {d.LastName}" }).ToList()
                    : allDrivers.Where(d => d.TruckCompany_Id == currentTruckCompany?.Id).Select(d => new SelectListItem { Value = d.Id.ToString(), Text = $"{d.FirstName} {d.LastName}" }).ToList(),
                VehicleOptions = User.IsInRole("Admin")
                    ? allVehicles.Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.LicensePlate }).ToList()
                    : allVehicles.Where(v => v.TruckCompany_Id == currentTruckCompany?.Id).Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.LicensePlate }).ToList()
            };            return View(model);
        }    [HttpPost]
    [Authorize(Roles = "TruckCompany,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignShipment(AssignShipmentViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        var allTruckCompanies = await _truckCompanyService.GetAllAsync();
        var currentTruckCompany = allTruckCompanies.FirstOrDefault(tc => tc.UserId == user.Id);
                ModelState.Remove("DriverOptions");
        ModelState.Remove("VehicleOptions");        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }                        model.DriverOptions = User.IsInRole("Admin")
                ? (await _driverService.GetAllAsync())
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = $"{d.FirstName} {d.LastName}" })
                    .ToList()
                : (await _driverService.GetAllAsync())
                    .Where(d => d.TruckCompany_Id == currentTruckCompany?.Id)
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = $"{d.FirstName} {d.LastName}" })
                    .ToList();            model.VehicleOptions = User.IsInRole("Admin")
                ? (await _vehicleService.GetAllAsync())
                    .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.LicensePlate })
                    .ToList()
                : (await _vehicleService.GetAllAsync())
                    .Where(v => v.TruckCompany_Id == currentTruckCompany?.Id)
                    .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.LicensePlate })
                    .ToList();            return View(model);
        }        var shipment = await _shipmentService.GetByIdAsync(model.ShipmentId);
        if (shipment == null || shipment.Status != "Available")
        {
            return NotFound();
        }        if (!User.IsInRole("Admin") && currentTruckCompany == null)
        {
            return Unauthorized();
        }
        await _shipmentService.AssignShipmentAsync(model.ShipmentId, model.DriverId, model.VehicleId, currentTruckCompany?.Id ?? 0);        return RedirectToAction("ListOfShipments");
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateShipment(int id)
    {
        var shipment = await _shipmentService.GetByIdAsync(id);
        if (shipment == null) return NotFound();
        var companyStocks = await _companyStockRepository.GetAllAsync();
        var existingAssignment = (await _companyStocksShipmentsRepository.GetAllAsync()).FirstOrDefault(cs => cs.Shipment_Id == shipment.Id);
        var model = new ShipmentViewModel
        {
            Shipment = shipment,
            Description = shipment.Description,
            Weight = shipment.Weight,
            FromAddress = shipment.FromAddress,
            ToAddress = shipment.ToAddress,
            PickUpDate = shipment.PickUpDate,
            DeliveryDate = shipment.DeliveryDate,
            Status = shipment.Status,
            SelectedFromCountry = shipment.FromCountry,
            SelectedFromTown = shipment.FromTown,
            SelectedToCountry = shipment.ToCountry,
            SelectedToTown = shipment.ToTown,
            SelectedCompanyStockId = existingAssignment?.CompanyStock_Id,
            FromCountries = new SelectList(PredefinedCountries, shipment.FromCountry),
            ToCountries = new SelectList(PredefinedCountries, shipment.ToCountry),
            FromTowns = new SelectList(PredefinedTowns.GetValueOrDefault(shipment.FromCountry, new List<string>()), shipment.FromTown),
            ToTowns = new SelectList(PredefinedTowns.GetValueOrDefault(shipment.ToCountry, new List<string>()), shipment.ToTown),
            CompanyStocks = new SelectList(companyStocks, "Id", "Name", existingAssignment?.CompanyStock_Id),
            PredefinedTowns = PredefinedTowns
        };
        return View(model);
    }    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateShipment(ShipmentViewModel model)
    {
        if (model.Shipment?.Id > 0 &&
            !string.IsNullOrEmpty(model.Description) &&
            model.Weight != 0 &&
            !string.IsNullOrEmpty(model.SelectedFromCountry) &&
            !string.IsNullOrEmpty(model.SelectedFromTown) &&
            !string.IsNullOrEmpty(model.FromAddress) &&
            !string.IsNullOrEmpty(model.SelectedToCountry) &&
            !string.IsNullOrEmpty(model.SelectedToTown) &&
            !string.IsNullOrEmpty(model.ToAddress) &&
            model.PickUpDate.HasValue &&
            model.DeliveryDate.HasValue &&
            !string.IsNullOrEmpty(model.Status))
        {
            var shipment = await _shipmentService.GetByIdAsync(model.Shipment.Id);
            if (shipment == null) return NotFound();
            shipment.Description = model.Description;
            shipment.Weight = model.Weight;
            shipment.FromCountry = model.SelectedFromCountry;
            shipment.FromTown = model.SelectedFromTown;
            shipment.FromAddress = model.FromAddress;
            shipment.ToCountry = model.SelectedToCountry;
            shipment.ToTown = model.SelectedToTown;
            shipment.ToAddress = model.ToAddress;
            shipment.PickUpDate = model.PickUpDate.Value;
            shipment.DeliveryDate = model.DeliveryDate.Value;
            shipment.Status = model.Status;
            await _shipmentService.UpdateAsync(shipment);
            await UpdateCompanyStockAssignment(model.SelectedCompanyStockId, shipment.Id);
            return RedirectToAction("ListOfShipments");
        }
        model = await PopulateDropdowns(model);
        return View(model);
    }    [Authorize(Roles = "Admin")]
    private async Task UpdateCompanyStockAssignment(int? companyStockId, int shipmentId)
    {
        var existingAssignment = (await _companyStocksShipmentsRepository.GetAllAsync()).FirstOrDefault(cs => cs.Shipment_Id == shipmentId);
        if (existingAssignment != null)
        {
            if (companyStockId.HasValue)
                existingAssignment.CompanyStock_Id = companyStockId.Value;
            else
                await _companyStocksShipmentsRepository.RemoveAsync(existingAssignment);
        }
        else if (companyStockId.HasValue)
        {
            await _companyStocksShipmentsRepository.AddAsync(new CompanyStocks_Shipments { Shipment_Id = shipmentId, CompanyStock_Id = companyStockId.Value });
        }
    }    private async Task<ShipmentViewModel> PopulateDropdowns(ShipmentViewModel model)
    {
        model.PredefinedTowns = PredefinedTowns;
        model.FromCountries = new SelectList(PredefinedCountries);
        model.ToCountries = new SelectList(PredefinedCountries);
        model.FromTowns = new SelectList(
            model.SelectedFromCountry != null && PredefinedTowns.ContainsKey(model.SelectedFromCountry)
                ? PredefinedTowns[model.SelectedFromCountry]
                : new List<string>(),
            model.SelectedFromTown
        );
        model.ToTowns = new SelectList(
            model.SelectedToCountry != null && PredefinedTowns.ContainsKey(model.SelectedToCountry)
                ? PredefinedTowns[model.SelectedToCountry]
                : new List<string>(),
            model.SelectedToTown
        );
        var companyStocks = await _companyStockRepository.GetAllAsync();
        model.CompanyStocks = new SelectList(companyStocks, "Id", "Name", model.SelectedCompanyStockId);
        return model;
    }    [HttpGet]
    public IActionResult GetTowns(string country)
    {
        if (string.IsNullOrEmpty(country) || !PredefinedTowns.ContainsKey(country))
        {
            return Json(new List<string>());
        }
        return Json(PredefinedTowns[country]);
    }    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var shipment = await _shipmentService.GetByIdAsync(id);
        if (shipment == null)
        {
            return Json(new { success = false, message = "Shipment not found" });
        }
        await _shipmentService.RemoveAsync(id);
        return Json(new { success = true });
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ENN_Cargo.Core;
using ENN_Cargo.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace ENN_Cargo.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ITruckCompanyService _truckCompanyService;
        public VehicleController(IVehicleService vehicleService, ITruckCompanyService truckCompanyService)
        {
            _vehicleService = vehicleService;
            _truckCompanyService = truckCompanyService;
        }
        [HttpGet]
        public async Task<IActionResult> ListOfVehicles(VehicleViewModel model)
        {
            var vehicles = await _vehicleService.GetAllAsync();
            var query = vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(model.SelectedBrand))
                query = query.Where(x => x.Brand == model.SelectedBrand);
            if (!string.IsNullOrEmpty(model.SelectedModel))
                query = query.Where(x => x.Model == model.SelectedModel);
            if (model.SelectedYear.HasValue)
                query = query.Where(x => x.Year == model.SelectedYear.Value);
            if (!string.IsNullOrEmpty(model.SelectedLicensePlateCountry))
                query = query.Where(x => x.LicensePlate.StartsWith(model.SelectedLicensePlateCountry));
            var predefinedBrands = new List<string> { "Volvo", "Scania", "MAN", "Mercedes-Benz", "DAF", "Iveco" };
            var predefinedModels = new List<string> { "FH16", "R500", "TGX", "Actros", "XF", "Stralis" };
            var predefinedYears = Enumerable.Range(1990, DateTime.Now.Year - 1989).Select(y => y.ToString()).ToList();
            var predefinedCountries = new List<string> { "US", "CA", "DE", "FR", "UK", "BG" };
            model.Vehicles = query.ToList();
            model.Brands = new SelectList(predefinedBrands, model.SelectedBrand);
            model.Models = new SelectList(predefinedModels, model.SelectedModel);
            model.Years = new SelectList(predefinedYears.Select(y => new SelectListItem { Value = y, Text = y }), "Value", "Text", model.SelectedYear);
            model.LicensePlateCountries = new SelectList(predefinedCountries, model.SelectedLicensePlateCountry);
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "TruckCompany,Admin")]
        public async Task<IActionResult> AddVehicle()
        {
            var truckCompanies = await _truckCompanyService.GetAllAsync();
            var model = new VehicleViewModel
            {
                TruckCompanies = new SelectList(truckCompanies, "Id", "Name")
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TruckCompany,Admin")]
        public async Task<IActionResult> AddVehicle(VehicleViewModel model)
        { 
            bool needsManualBind = model.Brand == null || model.Model == null || !model.Year.HasValue || model.LicensePlate == null || !model.SelectedTruckCompanyId.HasValue;
            if (needsManualBind)
            {
                model.Brand = Request.Form["Brand"];
                model.Model = Request.Form["Model"];
                model.Year = int.TryParse(Request.Form["Year"], out int year) ? year : null;
                model.LicensePlate = Request.Form["LicensePlate"];
                model.SelectedTruckCompanyId = int.TryParse(Request.Form["SelectedTruckCompanyId"], out int id) ? id : null;
                TryValidateModel(model); 
                ModelState.Clear(); 
                TryValidateModel(model); 
            }
            if (ModelState.IsValid)
            {
                var truckCompany = await _truckCompanyService.GetByIdAsync(model.SelectedTruckCompanyId.Value);
                if (truckCompany == null)
                {
                    model.TruckCompanies = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name", model.SelectedTruckCompanyId);
                    return View(model);
                }
                var vehicle = new Vehicle
                {
                    Brand = model.Brand ?? throw new ArgumentNullException("Brand cannot be null."),
                    Model = model.Model ?? throw new ArgumentNullException("Model cannot be null."),
                    Year = model.Year.Value, 
                    LicensePlate = model.LicensePlate ?? throw new ArgumentNullException("LicensePlate cannot be null."),
                    TruckCompany_Id = model.SelectedTruckCompanyId.Value 
                };
                await _vehicleService.AddAsync(vehicle);
                return RedirectToAction("ListOfVehicles");
            }
            model.TruckCompanies = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name", model.SelectedTruckCompanyId);
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "TruckCompany,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVehicle(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();
            var truckCompanies = await _truckCompanyService.GetAllAsync();
            var model = new VehicleViewModel
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year, 
                LicensePlate = vehicle.LicensePlate,
                SelectedTruckCompanyId = vehicle.TruckCompany_Id, 
                TruckCompanies = new SelectList(truckCompanies, "Id", "Name", vehicle.TruckCompany_Id)
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TruckCompany,Admin")]     
        public async Task<IActionResult> UpdateVehicle(int id, VehicleViewModel model) 
        {
            if (string.IsNullOrEmpty(model.Brand) || string.IsNullOrEmpty(model.Model) || !model.Year.HasValue || string.IsNullOrEmpty(model.LicensePlate) || !model.SelectedTruckCompanyId.HasValue)
            {
                model.Brand = Request.Form["Brand"];
                model.Model = Request.Form["Model"];
                model.Year = int.TryParse(Request.Form["Year"], out int year) ? year : null;
                model.LicensePlate = Request.Form["LicensePlate"];
                model.SelectedTruckCompanyId = int.TryParse(Request.Form["SelectedTruckCompanyId"], out int idFromForm) ? idFromForm : null;
                TryValidateModel(model); 
                ModelState.Clear(); 
                TryValidateModel(model); 
            }
            if (ModelState.IsValid)
            {
                var vehicle = await _vehicleService.GetByIdAsync(id);
                if (vehicle == null)
                    return NotFound();
                var truckCompany = await _truckCompanyService.GetByIdAsync(model.SelectedTruckCompanyId.Value);
                if (truckCompany == null)
                {
                    model.TruckCompanies = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name", model.SelectedTruckCompanyId);
                    return View(model);
                }
                vehicle.Brand = model.Brand ?? throw new ArgumentNullException("Brand cannot be null.");
                vehicle.Model = model.Model ?? throw new ArgumentNullException("Model cannot be null.");
                vehicle.Year = model.Year.Value;
                vehicle.LicensePlate = model.LicensePlate ?? throw new ArgumentNullException("LicensePlate cannot be null.");
                vehicle.TruckCompany_Id = model.SelectedTruckCompanyId.Value;
                await _vehicleService.UpdateAsync(vehicle);
                Console.WriteLine("Vehicle updated successfully!");
                return RedirectToAction("ListOfVehicles");
            }
            model.TruckCompanies = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name", model.SelectedTruckCompanyId);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TruckCompany,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetByIdAsync(id);
                if (vehicle == null)
                {
                    return Json(new { success = false, message = "Vehicle not found" });
                }
                await _vehicleService.RemoveAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error deleting vehicle: {ex.Message}" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListByTruckCompany(int truckCompanyId)
        {
            var vehicles = await _vehicleService.AllByAsync(x => x.TruckCompany_Id == truckCompanyId);
            return View(vehicles);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ENN_Cargo.Core;
using ENN_Cargo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ENN_Cargo.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ITruckCompanyService _truckCompanyService;
        public VehicleController(IVehicleService vehicleService, ITruckCompanyService truckCompanyService)
        {
            _vehicleService = vehicleService;
            _truckCompanyService = truckCompanyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListOfVehicles()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            var truckCompanies = await _truckCompanyService.GetAllAsync();
            var viewModel = new VehicleViewModel
            {
                Vehicles = vehicles.ToList(),
                Brands = new SelectList(vehicles.Select(x => x.Brand).Distinct()),
                Models = new SelectList(vehicles.Select(x => x.Model).Distinct()),
                Years = new SelectList(vehicles.Select(x=> x.Year).Distinct()),
                LicensePlateCountries = new SelectList(vehicles.Select(x => x.LicensePlate.Substring(0, 2)).Distinct())
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ListOfVehicles(VehicleViewModel model)
        {
            var vehicles = await _vehicleService.GetAllAsync();
            IQueryable<Vehicle> query = vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(model.SelectedBrand))
            {
                query = query.Where(x => x.Brand == model.SelectedBrand);
            }
            if (!string.IsNullOrEmpty(model.SelectedModel))
            {
                query = query.Where(x => x.Model == model.SelectedModel);
            }
            if (model.SelectedYear.HasValue)
            {
                query = query.Where(x => x.Year == model.SelectedYear.Value);
            }
            if (!string.IsNullOrEmpty(model.SelectedLicensePlateCountry))
            {
                query = query.Where(x => x.LicensePlate.StartsWith(model.SelectedLicensePlateCountry));
            }
            var filteredVehicles = query.ToList();
            var allVehicles = await _vehicleService.GetAllAsync();
            model.Vehicles = filteredVehicles;
            model.Brands = new SelectList(allVehicles.Select(x => x.Brand).Distinct());
            model.Models = new SelectList(allVehicles.Select(x => x.Model).Distinct());
            model.Years = new SelectList(allVehicles.Select(x => x.Year).Distinct());
            model.LicensePlateCountries = new SelectList(allVehicles.Select(v => v.LicensePlate.Substring(0, 2)).Distinct());
            return View(model);
        }
        public async Task<IActionResult> ListByTruckCompany(int truckCompanyId)
        {
            var vehicles = await _vehicleService.AllByAsync(x => x.TruckCompany_Id == truckCompanyId);
            return View(vehicles);
        }
    }
}
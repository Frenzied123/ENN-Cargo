using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ENN_Cargo.Core;
using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENN_Cargo.Controllers
{
    public class TruckCompanyController : Controller
    {
        private readonly ITruckCompanyService _truckCompanyService;
        public TruckCompanyController(ITruckCompanyService truckCompanyService)
        {
            _truckCompanyService = truckCompanyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListOfTruckCompanies()
        {
            var truckCompanies = await _truckCompanyService.GetAllAsync();
            var viewModel = new TruckCompanyViewModel
            {
                TruckCompanies = truckCompanies.ToList(),
                Countries = new SelectList(GetCountries()),
                Towns = new SelectList(Enumerable.Empty<string>())
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ListOfTruckCompanies(TruckCompanyViewModel model)
        {
            var filteredTruckCompanies = await _truckCompanyService.GetFilteredTruckCompaniesAsync(model.MinDrivers, model.MaxDrivers, model.MinVehicles, model.MaxVehicles,model.SelectedCountry, model.SelectedTown);

            model.TruckCompanies = filteredTruckCompanies.ToList();
            model.Countries = new SelectList(GetCountries());
            model.Towns = new SelectList(GetCitiesByCountry(model.SelectedCountry));

            return View(model);
        }
        private IEnumerable<string> GetCountries()
        {
            return new List<string> { "Bulgaria", "Germany", "USA" };
        }
        private IEnumerable<string> GetCitiesByCountry(string country)
        {
            return country switch
            {
                "Bulgaria" => new List<string> { "Sofia", "Kazanlak" },
                "Germany" => new List<string> { "Berlin", "Munich" },
                _ => Enumerable.Empty<string>()
            };
        }
        public async Task<IActionResult> ListByTruckCompany(int truckCompanyId)
        {
            var truckCompany = await _truckCompanyService.GetByIdAsync(truckCompanyId);
            if (truckCompany == null)
            {
                return NotFound();
            }
            return View(truckCompany);
        }
    }
}
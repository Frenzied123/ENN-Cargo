using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ENN_Cargo.Core;
using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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
        [HttpGet]
        public async Task<IActionResult> ListOfTruckCompanies(int? minDrivers, int? maxDrivers, int? minVehicles, int? maxVehicles, string selectedCountry, string selectedTown)
        {
            var filteredTruckCompanies = await _truckCompanyService.GetFilteredTruckCompaniesAsync(
                minDrivers, maxDrivers, minVehicles, maxVehicles, selectedCountry, selectedTown
            );

            var model = new TruckCompanyViewModel
            {
                TruckCompanies = filteredTruckCompanies?.Select(tc => new TruckCompanyViewModel.TruckCompanyItem
                {
                    Id = tc.Id,
                    Name = tc.Name,
                    Email = tc.User?.Email,
                    Address = tc.Address,
                    Country = tc.Country,
                    Town = tc.Town,
                    PhoneNumber = tc.User?.PhoneNumber,
                    Drivers = tc.Drivers?.ToList(),
                    Vehicles = tc.Vehicles?.ToList()
                }).ToList() ?? new List<TruckCompanyViewModel.TruckCompanyItem>(),
                MinDrivers = minDrivers,
                MaxDrivers = maxDrivers,
                MinVehicles = minVehicles,
                MaxVehicles = maxVehicles,
                SelectedCountry = selectedCountry,
                SelectedTown = selectedTown,
                Countries = new SelectList(GetCountries(), selectedCountry),
                Towns = new SelectList(GetCitiesByCountry(selectedCountry), selectedTown)
            };

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
        [HttpGet]
        public IActionResult AddTruckCompany()
        {
            var viewModel = new TruckCompanyViewModel
            {
                Countries = new SelectList(GetCountries()),
                Towns = new SelectList(Enumerable.Empty<string>())
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTruckCompany(TruckCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var truckCompany = new TruckCompany
                {
                    Name = model.Name,
                    Address = model.Address,
                    Country = model.SelectedCountry,
                    Town = model.SelectedTown,
                    User = new IdentityUser
                    {
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.Email
                    }
                };
                await _truckCompanyService.AddAsync(truckCompany);
                return RedirectToAction("ListOfTruckCompanies");
            }
            model.Countries = new SelectList(GetCountries());
            model.Towns = new SelectList(GetCitiesByCountry(model.SelectedCountry));
            return View(model);
        }
        [HttpGet]
        public IActionResult AddDriver(int truckCompanyId)
        {
            return Content($"Add Driver for Truck Company ID: {truckCompanyId} - Coming Soon!");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTruckCompany(int id)
        {
            var truckCompany = await _truckCompanyService.GetByIdAsync(id);
            if (truckCompany == null) return NotFound();
            var viewModel = new TruckCompanyViewModel
            {
                Name = truckCompany.Name,
                Email = truckCompany.User.Email,
                Address = truckCompany.Address,
                SelectedCountry = truckCompany.Country,
                SelectedTown = truckCompany.Town,
                PhoneNumber = truckCompany.User.PhoneNumber,
                Countries = new SelectList(GetCountries(), truckCompany.Country),
                Towns = new SelectList(GetCitiesByCountry(truckCompany.Country), truckCompany.Town)
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTruckCompany(int id, TruckCompanyViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Name) &&
                !string.IsNullOrEmpty(model.Email) &&
                !string.IsNullOrEmpty(model.Address) &&
                !string.IsNullOrEmpty(model.SelectedCountry) &&
                !string.IsNullOrEmpty(model.SelectedTown) &&
                !string.IsNullOrEmpty(model.PhoneNumber))
            {
                var truckCompany = await _truckCompanyService.GetByIdAsync(id);
                if (truckCompany == null)
                {
                    return NotFound();
                }
                truckCompany.Name = model.Name;
                truckCompany.Address = model.Address;
                truckCompany.Country = model.SelectedCountry;
                truckCompany.Town = model.SelectedTown;
                if (truckCompany.User == null)
                {
                    truckCompany.User = new IdentityUser { UserName = model.Email };
                }
                truckCompany.User.Email = model.Email;
                truckCompany.User.PhoneNumber = model.PhoneNumber;
                await _truckCompanyService.UpdateAsync(truckCompany);
                return RedirectToAction("ListOfTruckCompanies");
            }
            model.Countries = new SelectList(GetCountries());
            model.Towns = new SelectList(GetCitiesByCountry(model.SelectedCountry));
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var truckCompany = await _truckCompanyService.GetByIdAsync(id);
            if (truckCompany == null)
            {
                return NotFound();
            }
            await _truckCompanyService.RemoveAsync(id);
            return RedirectToAction("ListOfTruckCompanies");
        }
    }
}
using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace ENN_Cargo.Controllers
{
    public class CompanyStockController : Controller
    {
        private readonly ICompanyStockService _companyStockService;

        public CompanyStockController(ICompanyStockService companyStockService)
        {
            _companyStockService = companyStockService;
        }

        public async Task<IActionResult> ListOfCompaniesStock()
        {
            var companyStocks = await _companyStockService.GetAllAsync();
            var model = companyStocks.Select(cs => new CompanyStockViewModel
            {
                Id = cs.Id,
                Name = cs.Name,
                Email = cs.Email,
                PhoneNumber = cs.PhoneNumber,
                Address = cs.Address,
                SelectedCountry = cs.Country,
                SelectedTown = cs.Town
            }).ToList();

            return View(model);
        }
        public async Task<IActionResult> AddCompanyStock()
        {
            var model = await PopulateDropdowns(new CompanyStockViewModel());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompanyStock(CompanyStockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await PopulateDropdowns(model);
                return View(model);
            }

            var companyStock = new CompanyStock
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Country = model.SelectedCountry,
                Town = model.SelectedTown
            };

            await _companyStockService.AddAsync(companyStock);
            return RedirectToAction(nameof(ListOfCompaniesStock));
        }

        public async Task<IActionResult> UpdateCompanyStock(int id)
        {
            var companyStock = await _companyStockService.GetByIdAsync(id);
            if (companyStock == null)
            {
                return NotFound();
            }

            var model = await PopulateDropdowns(new CompanyStockViewModel
            {
                Id = companyStock.Id,
                Name = companyStock.Name,
                Email = companyStock.Email,
                PhoneNumber = companyStock.PhoneNumber,
                Address = companyStock.Address,
                SelectedCountry = companyStock.Country,
                SelectedTown = companyStock.Town
            });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCompanyStock(CompanyStockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await PopulateDropdowns(model);
                return View(model);
            }

            var stock = await _companyStockService.GetByIdAsync(model.Id.Value);
            if (stock == null)
            {
                return NotFound();
            }

            stock.Name = model.Name;
            stock.Email = model.Email;
            stock.PhoneNumber = model.PhoneNumber;
            stock.Address = model.Address;
            stock.Country = model.SelectedCountry;
            stock.Town = model.SelectedTown;

            await _companyStockService.UpdateAsync(stock);
            return RedirectToAction(nameof(ListOfCompaniesStock));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var companyStock = await _companyStockService.GetByIdAsync(id);
            if (companyStock == null)
            {
                return NotFound();
            }
            return View(companyStock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyStock = await _companyStockService.GetByIdAsync(id);
            if (companyStock != null)
            {
                await _companyStockService.RemoveAsync(companyStock.Id);
                return RedirectToAction(nameof(ListOfCompaniesStock));
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetTownsByCountry(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return PartialView("_TownDropdown", new List<SelectListItem>());
            }

            var towns = await _companyStockService.GetTownsByCountryAsync(country);
            var townList = towns.Select(t => new SelectListItem { Value = t, Text = t }).ToList();

            return PartialView("_TownDropdown", townList);
        }
        private async Task<CompanyStockViewModel> PopulateDropdowns(CompanyStockViewModel model)
        {
            var countryList = await _companyStockService.GetAllCountriesAsync();
            model.CountryList = countryList
                .Select(c => new SelectListItem { Value = c, Text = c })
                .Prepend(new SelectListItem { Value = "", Text = "Select a country", Selected = true })
                .ToList();

            model.TownList = !string.IsNullOrEmpty(model.SelectedCountry)
                ? (await _companyStockService.GetTownsByCountryAsync(model.SelectedCountry))
                    .Select(t => new SelectListItem { Value = t, Text = t, Selected = t == model.SelectedTown })
                    .ToList()
                : new List<SelectListItem>();

            return model;
        }
    }
}
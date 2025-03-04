using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Identity;
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
        [HttpGet]
        public async Task<IActionResult> ListOfCompaniesStock()
        {
            var companyStocks = await _companyStockService.GetAllAsync();
            var model = companyStocks?.Select(cs => new CompanyStockViewModel
            {
                Id = cs.Id,
                Name = cs.Name,
                Email = cs.User?.Email,
                PhoneNumber = cs.User?.PhoneNumber,
                Address = cs.Address,
                SelectedCountry = cs.Country,
                SelectedTown = cs.Town
            }).ToList() ?? new List<CompanyStockViewModel>();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddCompanyStock()
        {
            var model = await PopulateDropdowns(new CompanyStockViewModel());
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompanyStock(CompanyStockViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Name) &&
                !string.IsNullOrEmpty(model.Email) &&
                !string.IsNullOrEmpty(model.PhoneNumber) &&
                !string.IsNullOrEmpty(model.Address) &&
                !string.IsNullOrEmpty(model.SelectedCountry) &&
                !string.IsNullOrEmpty(model.SelectedTown))
            {
                var companyStock = new CompanyStock
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
                await _companyStockService.AddAsync(companyStock);
                return RedirectToAction("ListOfCompaniesStock");
            }
            model = await PopulateDropdowns(model);
            return View(model);
        }
        [HttpGet]
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
                Email = companyStock.User.Email,
                PhoneNumber = companyStock.User.PhoneNumber,
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
            if (model.Id.HasValue &&
                !string.IsNullOrEmpty(model.Name) &&
                !string.IsNullOrEmpty(model.Email) &&
                !string.IsNullOrEmpty(model.PhoneNumber) &&
                !string.IsNullOrEmpty(model.Address) &&
                !string.IsNullOrEmpty(model.SelectedCountry) &&
                !string.IsNullOrEmpty(model.SelectedTown))
            {
                var stock = await _companyStockService.GetByIdAsync(model.Id.Value);
                if (stock == null)
                {
                    return NotFound();
                }
                stock.Name = model.Name;
                stock.Address = model.Address;
                stock.Country = model.SelectedCountry;
                stock.Town = model.SelectedTown;
                if (stock.User == null)
                {
                    stock.User = new IdentityUser { UserName = model.Email };
                }
                stock.User.Email = model.Email;
                stock.User.PhoneNumber = model.PhoneNumber;
                await _companyStockService.UpdateAsync(stock);
                return RedirectToAction("ListOfCompaniesStock");
            }
            model = await PopulateDropdowns(model);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var companyStock = await _companyStockService.GetByIdAsync(id);
            if (companyStock == null)
            {
                return NotFound();
            }

            await _companyStockService.RemoveAsync(id);
            return RedirectToAction("ListOfCompaniesStock");
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
            model.CountryList = countryList.Select(c => new SelectListItem { Value = c, Text = c }).Prepend(new SelectListItem { Value = "", Text = "Select a country", Selected = true }).ToList();
            model.TownList = !string.IsNullOrEmpty(model.SelectedCountry)
           ? (await _companyStockService.GetTownsByCountryAsync(model.SelectedCountry)).Select(t => new SelectListItem { Value = t, Text = t, Selected = t == model.SelectedTown }).ToList(): new List<SelectListItem>();
            return model;
        }
    }
}
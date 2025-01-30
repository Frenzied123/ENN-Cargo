using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ENN_Cargo.Controllers
{
    public class CompanyStockController : Controller
    {
        private readonly ICompanyStockService _companyStockService;
        public CompanyStockController(ICompanyStockService companyStockService)
        {
            _companyStockService = companyStockService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Index(string sortOrder, string countryFilter, string townFilter)
        {
            var companyStocks = await _companyStockService.GetFilteredCompanyStocksAsync(sortOrder, countryFilter, townFilter);

            var viewModel = new CompanyStockViewModel
            {
                CompanyStocks = companyStocks,
                SortOrder = sortOrder,
                CountryFilter = countryFilter,
                TownFilter = townFilter
            };

            return View(viewModel);
        }
    }
}
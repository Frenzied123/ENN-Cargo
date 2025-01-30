using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ENN_Cargo.Controllers
{
    public class DriverController : Controller
    {
        private readonly IDriverService _driverService;
        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListOfDrivers()
        {
            var drivers = await _driverService.GetAllDriversAsync();
            var viewModel = new DriverViewModel
            {
                Drivers = drivers.ToList(),
                SortingOptions = new SelectList(new List<string> { "A-Z", "Z-A", "Low-High", "High-Low" })
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ListOfDrivers(DriverViewModel model)
        {
            var drivers = await _driverService.GetAllDriversAsync();
            IQueryable<Driver> query = drivers.AsQueryable();
            if (model.MinExperience.HasValue)
            {
                query = query.Where(x => x.Experience >= model.MinExperience.Value);
            }
            if (model.MaxExperience.HasValue)
            {
                query = query.Where(x => x.Experience <= model.MaxExperience.Value);
            }
            if (model.SortByExperience == "Low-High")
            {
                query = query.OrderBy(x => x.Experience);
            }
            else if (model.SortByExperience == "High-Low")
            {
                query = query.OrderByDescending(x => x.Experience);
            }
            if (model.SortByTruckCompany == "A-Z")
            {
                query = query.OrderBy(x => x.TruckCompany.Name);
            }
            else if (model.SortByTruckCompany == "Z-A")
            {
                query = query.OrderByDescending(x => x.TruckCompany.Name);
            }
            model.Drivers = query.ToList();
            model.SortingOptions = new SelectList(new List<string> { "A-Z", "Z-A", "Low-High", "High-Low" });
            return View(model);
        }
    }
}
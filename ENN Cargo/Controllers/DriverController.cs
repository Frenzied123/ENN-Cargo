using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class DriverController : Controller
{
    private readonly IDriverService _driverService;
    private readonly ITruckCompanyService _truckCompanyService;

    public DriverController(IDriverService driverService, ITruckCompanyService truckCompanyService)
    {
        _driverService = driverService;
        _truckCompanyService = truckCompanyService;
    }
    [HttpGet]
    public async Task<IActionResult> ListOfDrivers(int? minExperience, int? maxExperience, string sortByExperience, string sortByTruckCompany, int? selectedTruckCompanyId)
    {
        var drivers = await _driverService.GetFilteredDriversAsync(
            minExperience,
            maxExperience,
            sortByExperience,
            sortByTruckCompany,
            selectedTruckCompanyId
        );
        var model = new DriverViewModel
        {
            Drivers = drivers?.Select(d => new DriverViewModel.DriverItem
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName, 
                Email = d.User?.Email,
                Experience = d.Experience,
                PhoneNumber = d.User?.PhoneNumber,
                TruckCompany = d.TruckCompany != null ? new TruckCompany { Id = d.TruckCompany.Id, Name = d.TruckCompany.Name } : null
            }).ToList() ?? new List<DriverViewModel.DriverItem>(),
            MinExperience = minExperience,
            MaxExperience = maxExperience,
            SortByExperience = sortByExperience,
            SortByTruckCompany = sortByTruckCompany,
            SelectedTruckCompanyId = selectedTruckCompanyId,
            TruckCompanyList = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name", selectedTruckCompanyId),
            SortByExperienceOptions = new SelectList(new List<string> { "Low-High", "High-Low" }, sortByExperience),
            SortByTruckCompanyOptions = new SelectList(new List<string> { "A-Z", "Z-A" }, sortByTruckCompany)
        };

        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> AddDriver()
    {
        var truckCompanies = await _truckCompanyService.GetAllAsync();
        var model = new DriverViewModel
        {
            TruckCompanyList = new SelectList(truckCompanies, "Id", "Name"),
            SortByExperienceOptions = new SelectList(new List<string> { "Low-High", "High-Low" }),
            SortByTruckCompanyOptions = new SelectList(new List<string> { "A-Z", "Z-A" })
        };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddDriver(DriverViewModel model)
    {
        if (!string.IsNullOrEmpty(model.FirstName) &&
            !string.IsNullOrEmpty(model.LastName) &&
            !string.IsNullOrEmpty(model.Email) &&
            model.Experience >= 0 &&
            !string.IsNullOrEmpty(model.PhoneNumber))
        {
            var driver = new Driver
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Experience = model.Experience,
                TruckCompany_Id = model.SelectedTruckCompanyId ?? 0,
                User = new IdentityUser
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email
                }
            };
            await _driverService.AddAsync(driver);
            return RedirectToAction("ListOfDrivers");
        }
        model.TruckCompanyList = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name", model.SelectedTruckCompanyId);
        model.SortByExperienceOptions = new SelectList(new List<string> { "Low-High", "High-Low" });
        model.SortByTruckCompanyOptions = new SelectList(new List<string> { "A-Z", "Z-A" });
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> UpdateDriver(int id)
    {
        var driver = await _driverService.GetByIdAsync(id);
        if (driver == null)
        {
            return NotFound();
        }
        var viewModel = new DriverViewModel
        {
            Id = driver.Id,
            FirstName = driver.FirstName,
            LastName = driver.LastName,
            Email = driver.User.Email,
            Experience = driver.Experience,
            PhoneNumber = driver.User.PhoneNumber,
            SelectedTruckCompanyId = driver.TruckCompany_Id,
            TruckCompanyList = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name", driver.TruckCompany_Id),
            SortByExperienceOptions = new SelectList(new List<string> { "Low-High", "High-Low" }),
            SortByTruckCompanyOptions = new SelectList(new List<string> { "A-Z", "Z-A" })
        };
        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDriver(DriverViewModel model)
    {
        if (model.Id.HasValue &&
            !string.IsNullOrEmpty(model.FirstName) &&
            !string.IsNullOrEmpty(model.LastName) &&
            !string.IsNullOrEmpty(model.Email) &&
            model.Experience >= 0 &&
            !string.IsNullOrEmpty(model.PhoneNumber))
        {
            var driver = await _driverService.GetByIdAsync(model.Id.Value);
            if (driver == null)
            {
                return NotFound();
            }
            driver.FirstName = model.FirstName;
            driver.LastName = model.LastName;
            driver.Experience = model.Experience;
            driver.TruckCompany_Id = model.SelectedTruckCompanyId ?? 0;
            if (driver.User == null)
            {
                driver.User = new IdentityUser { UserName = model.Email };
            }
            driver.User.Email = model.Email;
            driver.User.PhoneNumber = model.PhoneNumber;
            await _driverService.UpdateAsync(driver);
            return RedirectToAction("ListOfDrivers");
        }
        model.TruckCompanyList = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name", model.SelectedTruckCompanyId);
        model.SortByExperienceOptions = new SelectList(new List<string> { "Low-High", "High-Low" });
        model.SortByTruckCompanyOptions = new SelectList(new List<string> { "A-Z", "Z-A" });
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var driver = await _driverService.GetByIdAsync(id);
        if (driver == null)
        {
            return NotFound();
        }
        await _driverService.RemoveAsync(id);
        return RedirectToAction("ListOfDrivers");
    }
}
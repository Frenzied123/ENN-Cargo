using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class DriverController : Controller
{
    private readonly IDriverService _driverService;
    private readonly ITruckCompanyService _truckCompanyService;

    public DriverController(IDriverService driverService, ITruckCompanyService truckCompanyService)
    {
        _driverService = driverService;
        _truckCompanyService = truckCompanyService;
    }

    public async Task<IActionResult> ListOfDrivers(int? minExperience, int? maxExperience, string sortByExperience, string sortByTruckCompany)
    {
        var driversList = await _driverService.GetAllDriversAsync();
        if (minExperience.HasValue)
        {
            driversList = driversList.Where(d => d.Experience >= minExperience.Value).ToList();
        }
        if (maxExperience.HasValue)
        {
            driversList = driversList.Where(d => d.Experience <= maxExperience.Value).ToList();
        }
        if (!string.IsNullOrEmpty(sortByExperience))
        {
            if (sortByExperience == "Low-High")
            {
                driversList = driversList.OrderBy(d => d.Experience).ToList();
            }
            else if (sortByExperience == "High-Low")
            {
                driversList = driversList.OrderByDescending(d => d.Experience).ToList();
            }
        }
        if (!string.IsNullOrEmpty(sortByTruckCompany))
        {
            if (sortByTruckCompany == "A-Z")
            {
                driversList = driversList.OrderBy(d => d.TruckCompany.Name).ToList();
            }
            else if (sortByTruckCompany == "Z-A")
            {
                driversList = driversList.OrderByDescending(d => d.TruckCompany.Name).ToList();
            }
        }
        var viewModel = new DriverViewModel
        {
            Drivers = driversList.ToList(),
            MinExperience = minExperience,
            MaxExperience = maxExperience,
            SortByExperience = sortByExperience,
            SortByTruckCompany = sortByTruckCompany,
            SortByExperienceOptions = new SelectList(new List<string> { "Low-High", "High-Low" }),
            SortByTruckCompanyOptions = new SelectList(new List<string> { "A-Z", "Z-A" })
        };

        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> ListOfDrivers(DriverViewModel model)
    {
        model.TruckCompanyList = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name");

        var drivers = await _driverService.GetFilteredDriversAsync(
            model.MinExperience,
            model.MaxExperience,
            model.SortByExperience,
            model.SortByTruckCompany,
            model.SelectedTruckCompanyId
        );

        model.Drivers = drivers.ToList();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AddDriver()
    {
        var truckCompanies = await _truckCompanyService.GetAllAsync();
        var model = new DriverViewModel
        {
        TruckCompanyList = new SelectList(truckCompanies, "Id", "Name"),
        SortingOptions = new SelectList(new List<string> { "A-Z", "Z-A", "Low-High", "High-Low" }),
        SortByExperienceOptions = new SelectList(new List<string> { "Low-High", "High-Low" }),
        SortByTruckCompanyOptions = new SelectList(new List<string> { "A-Z", "Z-A" })
        };
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> AddDriver(DriverViewModel model)
    {
        if (ModelState.IsValid)
        {
            var driver = new Driver
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Experience = model.Experience,
                PhoneNumber = model.PhoneNumber,
                TruckCompany_Id = model.SelectedTruckCompanyId ?? 0
            };

            await _driverService.AddAsync(driver);
            return RedirectToAction("ListOfDrivers");
        }

        model.TruckCompanyList = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name");
        return View(model);
    }
    public async Task<IActionResult> UpdateDriver(int id)
    {
        var driver = await _driverService.GetDriverByIdAsync(id);
        if (driver == null)
        {
            return NotFound();
        }

        var viewModel = new DriverViewModel
        {
            Id = driver.Id,
            FirstName = driver.FirstName,
            LastName = driver.LastName,
            Email = driver.Email,
            Experience = driver.Experience,
            PhoneNumber = driver.PhoneNumber,
            SelectedTruckCompanyId = driver.TruckCompany_Id,
            TruckCompanyList = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name")
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDriver(DriverViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.TruckCompanyList = new SelectList(await _truckCompanyService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        var driver = await _driverService.GetDriverByIdAsync(model.Id.Value);
        if (driver == null)
        {
            return NotFound();
        }

        driver.FirstName = model.FirstName;
        driver.LastName = model.LastName;
        driver.Email = model.Email;
        driver.Experience = model.Experience;
        driver.PhoneNumber = model.PhoneNumber;
        driver.TruckCompany_Id = model.SelectedTruckCompanyId ?? 0;

        await _driverService.UpdateAsync(driver);
        return RedirectToAction(nameof(ListOfDrivers));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var driver = await _driverService.GetDriverByIdAsync(id);
        if (driver == null)
        {
            return NotFound();
        }

        return View(driver);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var driver = await _driverService.GetDriverByIdAsync(id);
        if (driver != null)
        {
            await _driverService.RemoveAsync(id);
            return RedirectToAction(nameof(ListOfDrivers));
        }
        return NotFound();
    }
}
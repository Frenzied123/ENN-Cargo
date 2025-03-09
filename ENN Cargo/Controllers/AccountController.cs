using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;namespace ENN_Cargo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITruckCompanyService _truckCompanyService;
        private readonly IDriverService _driverService;
        private readonly ICompanyStockService _companyStockService;        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,RoleManager<IdentityRole> roleManager,ITruckCompanyService truckCompanyService,IDriverService driverService,ICompanyStockService companyStockService)
        {            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _truckCompanyService = truckCompanyService;
            _driverService = driverService;
            _companyStockService = companyStockService;
        }
        [HttpGet]
        public async Task<IActionResult>Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            { 
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Влизането е успешно";
                    return RedirectToAction("Index", "Home");
                }
                TempData["Error"] = "Invalid login attempt.";
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }
        public IActionResult Register() => View();
        [HttpGet]
        public async Task<IActionResult> RegisterForTruckCompanies()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RegisterForStockCompanies()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RegisterForDrivers()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterForTruckCompanies(RegisterForTruckCompany model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var truckCompany = new TruckCompany
                    {
                        Name = model.Name,
                        Address = model.Address,
                        Country = model.Country,
                        Town = model.Town,
                        UserId = user.Id
                    };
                  await _truckCompanyService.AddAsync(truckCompany);
                  await _userManager.AddToRoleAsync(user, "TruckCompany"); 
                  await _signInManager.SignInAsync(user, isPersistent: false);                   
                  return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterForStockCompanies(RegisterForCompanyStock model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var companyStock = new CompanyStock
                    {
                        Name = model.Name,
                        Address = model.Address,
                        Town = model.Town,
                        Country = model.Country,
                        UserId = user.Id
                    };
                  await _companyStockService.AddAsync(companyStock);
                  await _userManager.AddToRoleAsync(user, "ShipmentCompany");
                  await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterForDrivers(RegisterForDriver model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var driver = new Driver
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Experience = model.Experience,
                        UserId = user.Id 
                    };
                  await _driverService.AddAsync(driver);
                  await _userManager.AddToRoleAsync(user, "Driver");
                  await _signInManager.SignInAsync(user, isPersistent: false);
                  return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
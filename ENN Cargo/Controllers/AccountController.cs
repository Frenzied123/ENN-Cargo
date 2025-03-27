using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ENN_Cargo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITruckCompanyService _truckCompanyService;
        private readonly IDriverService _driverService;
        private readonly ICompanyStockService _companyStockService;
        private readonly IPendingRequest _pendingRequestService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITruckCompanyService truckCompanyService,
            IDriverService driverService,
            ICompanyStockService companyStockService,
            IPendingRequest pendingRequestService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _truckCompanyService = truckCompanyService;
            _driverService = driverService;
            _companyStockService = companyStockService;
            _pendingRequestService = pendingRequestService;
        }
        [HttpGet]
        public IActionResult Login()
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
                    var roles = await _userManager.GetRolesAsync(user);
                    string displayName = "";
                    if (roles.Contains("Admin"))
                    {
                        displayName = "Admin";
                    }
                    else if (roles.Contains("Driver"))
                    {
                        var drivers = await _driverService.GetAllAsync();
                        var driver = drivers.FirstOrDefault(d => d.UserId == user.Id);
                        if (driver != null)
                        {
                            displayName = $"{driver.FirstName} {driver.LastName}".Trim();
                        }
                    }
                    else if (roles.Contains("TruckCompany"))
                    {
                        var truckCompanies = await _truckCompanyService.GetAllAsync();
                        var truckCompany = truckCompanies.FirstOrDefault(tc => tc.UserId == user.Id);
                        if (truckCompany != null)
                        {
                            displayName = truckCompany.Name;
                        }
                    }
                    else if (roles.Contains("ShipmentCompany"))
                    {
                        var companyStocks = await _companyStockService.GetAllAsync();
                        var companyStock = companyStocks.FirstOrDefault(cs => cs.UserId == user.Id);
                        if (companyStock != null)
                        {
                            displayName = companyStock.Name;
                        }
                    }
                    if (string.IsNullOrEmpty(displayName))
                    {
                        displayName = model.Email.Split('@')[0];
                    }
                    HttpContext.Session.SetString("DisplayName", displayName);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }
        public IActionResult Register() => View();
        [HttpGet]
        public IActionResult RegisterForTruckCompanies()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterForStockCompanies()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterForDrivers()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterForTruckCompanies(RegisterForTruckCompany model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }
            var tempUserId = Guid.NewGuid().ToString();
            var request = new PendingRequest
            {
                Type = $"TruckCompanyRegistration: Email={model.Email}, Password={model.Password}, Name={model.Name}, Phone={model.PhoneNumber}, Address={model.Address}, Country={model.Country}, Town={model.Town}",
                UserId = tempUserId
            };
            await _pendingRequestService.AddPendingRequestAsync(request);
            return View("PendingApproval", new PendingApprovalViewModel
            {
                BackAction = "Login",
                BackController = "Account",
                Message = "Your truck company registration is waiting for admin approval."
            });
        }
        [HttpPost]
        public async Task<IActionResult> RegisterForStockCompanies(RegisterForCompanyStock model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }
            var tempUserId = Guid.NewGuid().ToString();
            var request = new PendingRequest
            {
                Type = $"ShipmentCompanyRegistration: Email={model.Email}, Password={model.Password}, Name={model.Name}, Phone={model.PhoneNumber}, Address={model.Address}, Country={model.Country}, Town={model.Town}",
                UserId = tempUserId
            };
            await _pendingRequestService.AddPendingRequestAsync(request);
            return View("PendingApproval", new PendingApprovalViewModel
            {
                BackAction = "Login",
                BackController = "Account",
                Message = "Your stock company registration is waiting for admin approval."
            });
        }
        [HttpPost]
        public async Task<IActionResult> RegisterForDrivers(RegisterForDriver model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }
            var tempUserId = Guid.NewGuid().ToString();
            var request = new PendingRequest
            {
                Type = $"DriverRegistration: Email={model.Email}, Password={model.Password}, Name={model.FirstName} {model.LastName}, Phone={model.PhoneNumber}, Experience={model.Experience}",
                UserId = tempUserId
            };
            await _pendingRequestService.AddPendingRequestAsync(request);
            return View("PendingApproval", new PendingApprovalViewModel
            {
                BackAction = "Login",
                BackController = "Account",
                Message = "Your driver registration is waiting for admin approval."
            });
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            Response.Cookies.Delete("ENN_Cargo_Auth", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var model = new UserSettingsViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = role
            };
            if (role == "Driver")
            {
                var driver = (await _driverService.GetAllAsync()).FirstOrDefault(d => d.UserId == user.Id);
                if (driver != null)
                {
                    model.FirstName = driver.FirstName;
                    model.LastName = driver.LastName;
                    model.Experience = driver.Experience;
                }
            }
            else if (role == "TruckCompany")
            {
                var truckCompany = (await _truckCompanyService.GetAllAsync()).FirstOrDefault(tc => tc.UserId == user.Id);
                if (truckCompany != null)
                {
                    model.Name = truckCompany.Name;
                    model.Address = truckCompany.Address;
                    model.Country = truckCompany.Country;
                    model.Town = truckCompany.Town;
                }
            }
            else if (role == "ShipmentCompany")
            {
                var companyStock = (await _companyStockService.GetAllAsync()).FirstOrDefault(cs => cs.UserId == user.Id);
                if (companyStock != null)
                {
                    model.Name = companyStock.Name;
                    model.Address = companyStock.Address;
                    model.Country = companyStock.Country;
                    model.Town = companyStock.Town;
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(UserSettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role == "Driver")
            {
                var driver = (await _driverService.GetAllAsync()).FirstOrDefault(d => d.UserId == user.Id);
                if (driver != null)
                {
                    driver.FirstName = model.FirstName;
                    driver.LastName = model.LastName;
                    driver.Experience = model.Experience;
                    await _driverService.UpdateAsync(driver);
                }
            }
            else if (role == "TruckCompany")
            {
                var truckCompany = (await _truckCompanyService.GetAllAsync()).FirstOrDefault(tc => tc.UserId == user.Id);
                if (truckCompany != null)
                {
                    truckCompany.Name = model.Name;
                    truckCompany.Address = model.Address;
                    truckCompany.Country = model.Country;
                    truckCompany.Town = model.Town;
                    await _truckCompanyService.UpdateAsync(truckCompany);
                }
            }
            else if (role == "ShipmentCompany")
            {
                var companyStock = (await _companyStockService.GetAllAsync()).FirstOrDefault(cs => cs.UserId == user.Id);
                if (companyStock != null)
                {
                    companyStock.Name = model.Name;
                    companyStock.Address = model.Address;
                    companyStock.Country = model.Country;
                    companyStock.Town = model.Town;
                    await _companyStockService.UpdateAsync(companyStock);
                }
            }
            TempData["Success"] = "Your settings have been updated successfully.";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
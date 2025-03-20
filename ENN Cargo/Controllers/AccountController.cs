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
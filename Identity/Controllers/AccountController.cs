using Identity.Models;
using Identity.Repository;
using Identity.Service.IService;
using Identity.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRoleService _roleService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IRoleService roleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleService = roleService;

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year, RegisterTime=DateTime.Now, LastLogin=DateTime.Now };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "base");
                    Response.Cookies.Append("UserId", $"{user.Id}");
                    return RedirectToAction("GetAllUsers", "Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl=null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        var user = await _userManager.FindByEmailAsync(model.Email);
                        user.LastLogin= DateTime.Now;
                        await _userManager.UpdateAsync(user);
                        var userRole = await _roleService.GetRoleName(user.Id);
                        if (userRole == "block")
                        {
                            ModelState.AddModelError("", "К сожалению вы были заблокированы");
                        }
                        else
                        {
                            Response.Cookies.Append("UserId", $"{user.Id}");
                            return RedirectToAction("GetAllUsers", "Users");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


    }
}

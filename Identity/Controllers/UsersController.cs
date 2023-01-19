using Identity.Models;
using Identity.Service;
using Identity.Service.IService;
using Identity.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task4.Service.IService;

namespace Identity.Controllers
{
    //[Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public UsersController(UserManager<User> userManager, IRoleService roleService, IUserService userService)
        {
            _userManager = userManager;
            _roleService = roleService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUser();
            return View(users);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Create(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
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

        public async Task<IActionResult> Edit(string id)
        {
            EditUserViewModel model = await _userService.Edit(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Edit(model);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            await _userService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUsers(List<string> userId)
        {
            var check = false;
            foreach(var id in userId)
            {
                await _userService.Delete(id);
                if (Request.Cookies["UserId"] == id)
                {
                    check= true;
                }
            }
            if (check) { RedirectToRoute(new { controller = "Account", action = "Logout" }); }
            else
            {
                return RedirectToAction("GetAllUsers");
            }
            return BadRequest();
        }
    }
}

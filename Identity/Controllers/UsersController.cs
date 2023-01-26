using Identity.Models;
using Identity.Repository;
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
        private readonly IRoleRepository _roleRepository;

        public UsersController(UserManager<User> userManager, IRoleService roleService, IUserService userService, IRoleRepository roleRepository)
        {
            _userManager = userManager;
            _roleService = roleService;
            _userService = userService;
            _roleRepository = roleRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var model = new UserModel()
            {
                Checkboxes = await _userService.GetAllUser()
            };
            return View(model);
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
            if (check) { RedirectToRoute(new { controller = "Account", action = "Login" }); }
            else
            {
                return RedirectToAction("GetAllUsers");
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> WhatToDo(UserModel model, string buttonName)
        {
            if (model.UniqueName != null && buttonName != null)
            {
                var check = false;
                switch (buttonName)
                {
                    case "btnBlock":
                        foreach (var id in model.UniqueName)
                        {
                            await _roleRepository.BlockUser(id);

                            if (Request.Cookies["UserId"] == id)
                            {
                                check = true;
                            }

                        }
                        if (check) {return RedirectToAction("Login","Account"); }

                        break;
                    case "btnUnlock":
                        foreach (var id in model.UniqueName)
                        {
                           await _roleRepository.UnlockUser(id);
                        }
                        break;
                    case "btnDelete":
                        foreach(var id in model.UniqueName)
                        {
                            await _userService.Delete(id);
                            if (Request.Cookies["UserId"] == id)
                            {
                                check= true;
                            }
                        }
                        if (check) { return RedirectToAction("Login","Account"); }
                        break;
                }
            }
            return RedirectToAction("GetAllUsers");
        }
    }
}

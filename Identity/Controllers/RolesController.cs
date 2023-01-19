using Identity.Models;
using Identity.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRoleRepository _roleRepository;
        public RolesController(UserManager<User> userManager,
                               SignInManager<User> signInManager,
                               IRoleRepository roleRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleRepository = roleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(List<string> userId)
        {
            var check = false;
            if (userId != null)
            {
                foreach (var id in userId)
                {
                    await _roleRepository.BlockUser(id);

                    if (Request.Cookies["UserId"] == id)
                    {
                        check = true;
                    }

                }
                if (check) { RedirectToRoute(new { controller = "Account", action = "Logout" }); }
            }
            return RedirectToAction("GetAllUsers", "Users");
        }
        public async Task<IActionResult> UnlockUser(List<string> userId)
        {
            if (userId != null)
            {
                foreach (var id in userId)
                {
                    await _roleRepository.UnlockUser(id);
                }
            }
            return RedirectToAction("GetAllUsers", "Users");
        }
    }
}

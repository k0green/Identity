using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public RoleRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "base");
                await _signInManager.RefreshSignInAsync(user);
                await _userManager.AddToRoleAsync(user, "block");
                await _signInManager.RefreshSignInAsync(user);
            }
        }
        public async Task UnlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "block");
                await _signInManager.RefreshSignInAsync(user);
                await _userManager.AddToRoleAsync(user, "base");
                await _signInManager.RefreshSignInAsync(user);

            }
        }
    }
}

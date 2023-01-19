using Identity.Models;
using Identity.Repository;
using Identity.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4.Service.IService;

namespace Identity.Service
{
    public class RoleService : IRoleService
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        IRoleRepository _roleRepository;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IRoleRepository roleRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleRepository = roleRepository;
        }
        public async Task<string> GetRoleName(string userId)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            // получем список ролей пользователя
            var userRoles = await _userManager.GetRolesAsync(user);
           
            
            return userRoles[0];
        }

        public async Task AddRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
            }
        }

        public async Task BlockUser(string id)
        {
            await _roleRepository.BlockUser(id);
        }
        public async Task UnlockUser(string id)
        {
            await _roleRepository.UnlockUser(id);
        }
    }
}

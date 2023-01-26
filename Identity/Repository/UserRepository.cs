using Azure.Core;
using Identity.Models;
using Identity.Repository.IRepository;
using Identity.Service.IService;
using Identity.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task4.Service.IService;

namespace Identity.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;

        public UserRepository(UserManager<User> userManager, IRoleService roleService)
        {
            _userManager = userManager;
            _roleService = roleService;
        }

        public async Task<List<UserViewModel>> GetAllUser()
        {
            var users = _userManager.Users.Select(x => new UserViewModel
            {
                IsChecked = false,
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                LastLoginTime = (DateTime)x.LastLogin,
                RegisterTime = (DateTime)x.RegisterTime
            }).ToList();
            foreach (var user in users)
            {
                user.RoleName = await _roleService.GetRoleName(user.Id);
            }
            return users;
        }

        public async Task<IdentityResult> Create(CreateUserViewModel model)
        {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                var result = await _userManager.CreateAsync(user, model.Password);
                return result;

        }

        public async Task<EditUserViewModel> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Year = user.Year };
            return model;
        }

        public async Task<IdentityResult> Edit(EditUserViewModel model)
        {
                User user = await _userManager.FindByIdAsync(model.Id);
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.Year = model.Year;
                    var result = await _userManager.UpdateAsync(user);
                        return result;
        }

        public async Task Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
        }
    }
}

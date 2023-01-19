using Identity.Models;
using Identity.Repository.IRepository;
using Identity.Service.IService;
using Identity.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4.Service.IService;

namespace Identity.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository= userRepository;
        }
        public async Task<List<UserViewModel>> GetAllUser()
        {
            var users = await _userRepository.GetAllUser();
            return users;
        }

        public async Task<IdentityResult> Create(CreateUserViewModel model)
        {
            var result = await _userRepository.Create(model);
            return result;

        }

        public async Task<EditUserViewModel> Edit(string id)
        {
            EditUserViewModel model = await _userRepository.Edit(id);
            return model;
        }

        public async Task<IdentityResult> Edit(EditUserViewModel model)
        {
            var result = await _userRepository.Edit(model);
            return result;
        }

        public async Task Delete(string id)
        {
            await _userRepository.Delete(id);
        }
    }
}


using Identity.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Task4.Service.IService
{
    public interface IUserService
    {
        public Task Delete(string id);
        public Task<IdentityResult> Edit(EditUserViewModel model);
        public Task<EditUserViewModel> Edit(string id);
        public Task<IdentityResult> Create(CreateUserViewModel model);
        public Task<List<UserViewModel>> GetAllUser();
    }
}

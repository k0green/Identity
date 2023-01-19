using Identity.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Identity.Repository.IRepository
{
    public interface IUserRepository
    {
        public Task Delete(string id);
        public Task<IdentityResult> Edit(EditUserViewModel model);
        public Task<EditUserViewModel> Edit(string id);
        public Task<IdentityResult> Create(CreateUserViewModel model);
        public Task<List<UserViewModel>> GetAllUser();
    }
}

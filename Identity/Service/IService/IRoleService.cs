
namespace Identity.Service.IService
{
    public interface IRoleService
    {
        public Task<string> GetRoleName(string userId);
        public Task AddRole(string name);
        public Task BlockUser(string id);
        public Task UnlockUser(string id);
    }
}

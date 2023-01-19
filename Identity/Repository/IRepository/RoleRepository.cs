namespace Identity.Repository
{
    public interface IRoleRepository
    {
        public Task BlockUser(string id);
        public Task UnlockUser(string id);
    }
}

namespace Identity.ViewModel
{
    public class UserViewModel
    {
        public bool IsChecked { get; set; }
        public string Id { get; set; }
        public string UserName { get; set;}
        public string Email { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string RoleName { get; set; }

    }
}

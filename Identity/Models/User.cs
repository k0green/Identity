using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    public class User:IdentityUser
    {
        public int Year { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? RegisterTime { get; set; }
    }
}

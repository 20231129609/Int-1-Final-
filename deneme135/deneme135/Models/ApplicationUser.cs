using Microsoft.AspNetCore.Identity;

namespace deneme135.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserName { get; set; }
        public override string Email { get; set; } = null; // Email'i null yapabilirsiniz
        public string PasswordHash { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;

namespace MyImageApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }  // 👈 nova coluna
    }
}

using Microsoft.AspNetCore.Identity;

namespace E_commerce_System.Entities
{
 
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}

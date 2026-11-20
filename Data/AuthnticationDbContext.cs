using E_commerce_System.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_System.Data
{
    public class AuthnticationDbContext:IdentityDbContext<ApplicationUser>
    {
        public AuthnticationDbContext(DbContextOptions<AuthnticationDbContext> options):base(options)
        {
            
        }
    }
}

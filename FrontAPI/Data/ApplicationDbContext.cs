using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FrontAPI.Data.Models;


namespace FrontAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Location> Locations => Set<Location>();
        public DbSet<State> States => Set<State>();
    }
}

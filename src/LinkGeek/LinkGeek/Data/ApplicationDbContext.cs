using LinkGeek.AppIdentity;
using LinkGeek.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static DbContextOptions<ApplicationDbContext> Options;
        
        public DbSet<Game> Game { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Options = options;
        }
        
        public ApplicationDbContext()
            : base(Options)
        {
        }
    }
}
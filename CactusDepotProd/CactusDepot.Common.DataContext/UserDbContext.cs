using CactusDepot.Shared.Models.Administration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CactusDepot.Shared.DataContext
{
    public class UserDbContext : IdentityDbContext
    {
        /// <summary>
        /// Constructor 
        /// pass the same "options" to the "base" class
        /// </summary>
        /// <param name="options"></param>
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        //ApplicationUser : IdentityUser
        public DbSet<ApplicationUser> AspNetUsers { get; set; } = null!;

    }
}

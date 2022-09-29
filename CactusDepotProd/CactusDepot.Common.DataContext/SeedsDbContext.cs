using CactusDepot.Shared.Models.Seeds;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace CactusDepot.Shared.DataContext
{
    public class SeedsDbContext : DbContext
    {
        /// <summary>
        /// Constructor 
        /// pass the same "options" to the "base" class
        /// </summary>
        /// <param name="options"></param>
        public SeedsDbContext(DbContextOptions<SeedsDbContext> options)
            : base(options)
        {
        }
        //telling Entity Framework Core that you want to store Seeds entities in a table called CactusSeeds
        public DbSet<CactusSeed> CactusSeeds { get; set; } = null!;

    }
}

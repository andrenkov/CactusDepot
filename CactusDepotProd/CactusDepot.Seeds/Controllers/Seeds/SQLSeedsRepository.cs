using CactusDepot.Shared.DataContext;
using CactusDepot.Shared.Models.Seeds;

namespace CactusDepot.Seeds.Controllers
{
    public class SQLSeedsRepository : ISeedsRepository
    {
        private readonly SeedsDbContext dbcontext;

        public SQLSeedsRepository(SeedsDbContext _context)
        {
            dbcontext = _context;//constructor injection
        }

        public CactusSeed Add(CactusSeed seed)
        {
            dbcontext.CactusSeeds.Add(seed);
            dbcontext.SaveChanges();
            return seed;
        }

        public async Task<int> CountSeeds()
        {
            return await Task.FromResult(dbcontext.CactusSeeds.Count());
        }
        public CactusSeed? Delete(int? Id)
        {
            CactusSeed? seed = null;
            if (Id is not null)
            {
                seed = dbcontext.CactusSeeds.Find(Id);
                if (seed is not null)
                {
                    dbcontext.CactusSeeds.Remove(seed);
                    dbcontext.SaveChanges();
                }
            }
            return seed;
        }

        public IEnumerable<CactusSeed> GetAllSeeds()
        {
            return dbcontext.CactusSeeds;
        }

        public CactusSeed? GetSeed(int Id)
        {
            return dbcontext.CactusSeeds.Find(Id);
        }

        public CactusSeed Update(CactusSeed seedChanges)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<CactusSeed> seed = dbcontext.CactusSeeds.Attach(seedChanges);
            seed.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbcontext.SaveChanges();
            return seedChanges;
        }


    }
}

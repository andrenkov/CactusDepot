using CactusDepot.Shared.Models.Seeds;

namespace CactusDepot.Seeds.Controllers
{
    public interface ISeedsRepository
    {
        CactusSeed? GetSeed(int Id);
        IEnumerable<CactusSeed> GetAllSeeds();
        CactusSeed Add(CactusSeed seed);
        CactusSeed Update(CactusSeed seedChanges);
        CactusSeed? Delete(int? Id);
    }
}
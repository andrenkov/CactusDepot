using CactusDepot.Shared;
using CactusDepot.Shared.Models.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CactusDepot.Seeds.Controllers
{
    [Authorize(Roles = "Administrator,Manager,SuperAdmin")]
    public partial class SeedsController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CatalogByYearNum(string SearchYear, string SearchNum)
        {
            _logger.LogInformation("Getting seeds catalog. CatalogByYearNum()");
            SharedUtil.WriteLogToConsole("SeedsController", "GET.CatalogByYearNum started");

            IQueryable<CactusSeed> seeds = from m in _context.CactusSeeds orderby m.SeedId descending select m;
            IQueryable<CactusSeed>? res = null;

            #region Get all for Year
            if (!string.IsNullOrEmpty(SearchYear) && string.IsNullOrEmpty(SearchNum))
            {
                if (int.TryParse(SearchYear, out int year) && (year > 0))
                {
                     res = seeds.Where(s => s.SeedYear == year);
                }
            }
            #endregion
            #region Get One for Year and Num
            if (!string.IsNullOrEmpty(SearchYear) && !string.IsNullOrEmpty(SearchNum))
            {
                if (int.TryParse(SearchYear, out int year) && (year > 0))
                {
                    res = seeds.Where(s => (s.SeedYear == year) && ((s.Parent1CatalogNum == SearchNum) || (s.Parent2CatalogNum == SearchNum)));
                }
            }
            #endregion

            if ((res is not null) && (res.ToList().Count > 0))
            {
                SharedUtil.WriteLogToConsole("SeedsController", "GET.CatalogByYearNum returning");
                return View("CatalogList", await res.ToListAsync());
            }

            return View("NotFound");
        }
    }
}

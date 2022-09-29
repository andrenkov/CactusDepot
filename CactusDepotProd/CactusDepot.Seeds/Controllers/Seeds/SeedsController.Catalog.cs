using CactusDepot.Shared;
using CactusDepot.Shared.Models;
using CactusDepot.Shared.Models.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CactusDepot.Seeds.Controllers
{
    public partial class SeedsController : Controller
    {
        #region Catalog page
        // GET: seeds to View only
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CatalogList(string searchString)
        {
            _logger.LogInformation("Getting seeds catalog. Single page");
            SharedUtil.WriteLogToConsole("SeedsController", "GET.CatalogList() started");

            SharedUtil.WriteLogToConsole("SeedsController", $"GET.CatalogList() _context.CactusSeeds.Count : {_context.CactusSeeds.Count()}"); ;

            IQueryable<CactusSeed> seeds = from m in _context.CactusSeeds orderby m.SeedCollectedDate descending, m.SeedId descending select m;
            IQueryable<CactusSeed>? res = null;

            SharedUtil.WriteLogToConsole("SeedsController", "IQueryable<CactusSeed> completed");

            if (!string.IsNullOrEmpty(searchString))
            {
                res = seeds.Where(s => s.SeedName.Contains(searchString));
            }

            if ((res is not null) && (res.ToList().Count > 0))
            {
                SharedUtil.WriteLogToConsole("SeedsController", "GET.CatalogList() returning");
                return View(await res.ToListAsync());
            }

            return View("NotFound");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CatalogPages(string searchString, int p = 1)
        {
            _logger.LogInformation("Getting seeds with pages");
            SharedUtil.WriteLogToConsole("SeedsController", "GET.CatalogPages() started");
            SharedUtil.WriteLogToConsole("SeedsController", $"GET.CatalogList() _context.CactusSeeds.Count : {_context.CactusSeeds.Count()}"); ;
            IQueryable<CactusSeed> seedsOrderBy = from m in _context.CactusSeeds orderby m.SeedCollectedDate descending, m.SeedId descending select m;
            SharedUtil.WriteLogToConsole("SeedsController", "GET IQueryable<CactusSeed> completed");

            PagedResult<CactusSeed>? seeds = seedsOrderBy.GetPaged(p, 12);//15s delay here!!!!!
            SharedUtil.WriteLogToConsole("SeedsController", "GET PagedResult<CactusSeed> completed");
            if (!string.IsNullOrEmpty(searchString))
            {
                SharedUtil.WriteLogToConsole("SeedsController", "GET.CatalogPages() search started");
                seeds = _context.CactusSeeds.Where(s => s.SeedName.Contains(searchString)).GetPaged(p, 12);
                SharedUtil.WriteLogToConsole("SeedsController", "GET.CatalogPages() search completed");
            }

            if (seeds is not null)
            {
                SharedUtil.WriteLogToConsole("SeedsController", "GET.CatalogPages() returning");
                return View(seeds);
            }

            return View("NotFound");
        }
        #endregion
    }
}

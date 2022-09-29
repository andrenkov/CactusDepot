using CactusDepot.Shared;
using CactusDepot.Shared.DataContext;
using CactusDepot.Shared.Models;
using CactusDepot.Shared.Models.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CactusDepot.Seeds.Controllers
{
    [Authorize(Roles = "Administrator,Manager,SuperAdmin")]
    public partial class SeedsController : Controller
    {
        #region Details
        //[Authorize]
        [AllowAnonymous]
        [HttpGet]
        // GET: Seeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _logger.LogInformation("SeedsController.Details.Id");
            SharedUtil.WriteLogToConsole("SeedsController", "GET.Details(id)");
            CactusSeed? seeds = await _context.CactusSeeds.FirstOrDefaultAsync(m => m.SeedId == id);
            if (seeds == null)
            {
                return NotFound();
            }

            return View(seeds);
        }
        #endregion

    }
}

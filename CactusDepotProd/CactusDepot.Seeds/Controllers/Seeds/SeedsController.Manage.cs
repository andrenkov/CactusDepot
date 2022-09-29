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
        #region Create
        public IActionResult Create()
        {
            return View(new CactusSeed());
        }
        // POST: Seeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeedId,SeedName,Parent1CatalogNum,Parent2CatalogNum,SeedNote,SeedCollectedDate,SeedSource,SeedSeedsQty,SeedYear," +
                                                      "SeedCatalogNum,SeedLastSowedYear,SeedAvailable,SeedRating")] CactusSeed seeds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seeds);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User added seeds.", seeds);
                SharedUtil.WriteLogToConsole("SeedsController", "POST.Create()");
                return RedirectToAction(nameof(CatalogPages));
            }
            return View(seeds);
        }
        #endregion

        #region Edit
        // GET: Seeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SharedUtil.WriteLogToConsole("SeedsController", "GET.Edit() started");
            CactusSeed? seeds = await _context.CactusSeeds.FindAsync(id);
            if (seeds is null)
            {
                return NotFound();
            }
            SharedUtil.WriteLogToConsole("SeedsController", "GET.Edit() returning");
            return View(seeds);
        }

        // POST: Seeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeedId,SeedName,Parent1CatalogNum,Parent2CatalogNum,SeedNote,SeedCollectedDate,SeedSource,SeedSeedsQty," +
                                                            "SeedYear,SeedCatalogNum,SeedLastSowedYear,SeedAvailable,SeedRating")] CactusSeed seeds)
        {
            if (id != seeds.SeedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seeds);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User edited seeds.", seeds);
                    SharedUtil.WriteLogToConsole("SeedsController", "POST.Edit() started");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeedExists(seeds.SeedId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                SharedUtil.WriteLogToConsole("SeedsController", "POST.Edit() returning");
                return RedirectToAction(nameof(CatalogPages));//model valid
            }
            return View(seeds);//model invalid
        }
        #endregion

        #region Delete
        // GET: Seeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SharedUtil.WriteLogToConsole("SeedsController", "GET.Delete()");
            CactusSeed? seeds = await _context.CactusSeeds.FirstOrDefaultAsync(m => m.SeedId == id);
            if (seeds is null)
            {
                return NotFound();
            }

            return View(seeds);
        }

        // POST: Seeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CactusSeed? seeds = await _context.CactusSeeds.FindAsync(id);
            if (seeds is not null)
            {
                _context.CactusSeeds.Remove(seeds);
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation("User deleted seeds.", seeds);
            SharedUtil.WriteLogToConsole("SeedsController", "POST.Delete()");
            return RedirectToAction(nameof(CatalogPages));
        }

        private bool SeedExists(int id)
        {
            return _context.CactusSeeds.Any(e => e.SeedId == id);
        }
        #endregion

    }
}

using CactusDepot.Shared;
using CactusDepot.Shared.DataContext;
using CactusDepot.Shared.Models;
using CactusDepot.Shared.Models.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CactusDepot.Seeds.Controllers
{
    //[AllowAnonymous]
    //[Authorize]
    //[Authorize("ApprovedUser")]
    //[Authorize(Roles = "Administrator")]

    [Authorize(Roles = "Administrator,Manager,SuperAdmin")]
    public partial class SeedsController : Controller
    {
        public const string ErrorMessageILoggerFactoryIsNull = "ILoggerFactory is null";

        private readonly SeedsDbContext _context;
        private readonly ILogger<CactusSeed> _logger;
        private readonly IWebHostEnvironment _hostingWebEnvironment;
        private readonly IHostEnvironment _hostingAppEnvironment;
        private readonly string _uploadsFolder;
        //private readonly ISeedsRepository _seedsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public SeedsController(SeedsDbContext context, ILogger<CactusSeed> logger, IWebHostEnvironment hostingEnvironment, IHostEnvironment hostingAppEnvironment)//, ISeedsRepository seedsRepository)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("SeedsController init");
            SharedUtil.WriteLogToConsole("SeedsController", "Constructor started");
            _hostingWebEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _hostingAppEnvironment = hostingAppEnvironment ?? throw new ArgumentNullException(nameof(hostingAppEnvironment));
            _uploadsFolder = Path.Combine(_hostingAppEnvironment.ContentRootPath, "Db", "Images");//path "/app/Db/Images"
            //_seedsRepository = seedsRepository;
            SharedUtil.WriteLogToConsole("SeedsController", "Constructor completed");
        }

        #region Index

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(string searchString, int p = 1)
        {
            _logger.LogInformation("SeedsController.Index()");
            SharedUtil.WriteLogToConsole("SeedsController", "GET.Index() started");
            IQueryable<CactusSeed> seedsOrderBy = from m in _context.CactusSeeds orderby m.SeedCollectedDate descending select m;

            PagedResult<CactusSeed> seeds = seedsOrderBy.GetPaged(p, 12);
            if (!string.IsNullOrEmpty(searchString))
            {
                seeds = _context.CactusSeeds.Where(s => s.SeedName.Contains(searchString)).GetPaged(p, 12);
            }
            SharedUtil.WriteLogToConsole("SeedsController", "GET.Index() returning");
            return View(seeds);
        }
        #endregion


        #region Json list - testing an API style of return
        //[AllowAnonymous]
        [Authorize]
        [HttpGet]
        public JsonResult GetSeedsJson(string searchString)
        {
            IQueryable<CactusSeed> seeds = from m in _context.CactusSeeds select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                seeds = seeds.Where(s => s.SeedName.Contains(searchString));
            }
            return Json(seeds);
        }
        #endregion
        
    }
}

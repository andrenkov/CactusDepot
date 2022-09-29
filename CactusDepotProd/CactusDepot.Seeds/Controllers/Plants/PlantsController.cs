using CactusDepot.Shared;
using CactusDepot.Shared.DataContext;
using CactusDepot.Shared.Models;
using CactusDepot.Shared.Models.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CactusDepot.Seeds.Controllers.Plants
{
    [Authorize(Roles = "Administrator,Manager,SuperAdmin")]
    public partial class PlantsController : Controller
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
        public PlantsController(SeedsDbContext context, ILogger<CactusSeed> logger, IWebHostEnvironment hostingEnvironment, IHostEnvironment hostingAppEnvironment)//, ISeedsRepository seedsRepository)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("PlantsController init");
            SharedUtil.WriteLogToConsole("PlantsController", "Constructor started");
            _hostingWebEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _hostingAppEnvironment = hostingAppEnvironment ?? throw new ArgumentNullException(nameof(hostingAppEnvironment));
            _uploadsFolder = Path.Combine(_hostingAppEnvironment.ContentRootPath, "Db", "Images");//path "/app/Db/Images"
            //_seedsRepository = seedsRepository;
            SharedUtil.WriteLogToConsole("PlantsController", "Constructor completed");
        }

        #region Index

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(string searchString, int p = 1)
        {
            _logger.LogInformation("PlantsController.Index()");
            SharedUtil.WriteLogToConsole("PlantsController", "GET.Index() started");

            //IQueryable<CactusSeed> seedsOrderBy = from m in _context.CactusSeeds orderby m.SeedCollectedDate descending select m;

            //PagedResult<CactusSeed> seeds = seedsOrderBy.GetPaged(p, 12);
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    seeds = _context.CactusSeeds.Where(s => s.SeedName.Contains(searchString)).GetPaged(p, 12);
            //}
            SharedUtil.WriteLogToConsole("PlantsController", "GET.Index() returning");
            //return View(seeds);
            return View("UnderContruction");
        }
        #endregion
    }
}

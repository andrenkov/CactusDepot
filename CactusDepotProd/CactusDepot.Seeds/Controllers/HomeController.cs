using CactusDepot.Shared.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CactusDepot.Seeds.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home page hit.");
            return View();
        }

        public IActionResult About()
        {
            _logger.LogInformation("About page hit.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogWarning($"Home controller error '{HttpContext.TraceIdentifier}'");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using NuGet.Protocol;

namespace CactusDepot.Seeds.Controllers
{
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IHealthCheck _service;
        private readonly HealthCheckContext _context;

        public HealthController(ILogger<HealthController> logger, IHealthCheck service, HealthCheckContext context)
        {
            _logger = logger;
            _service = service;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var report = await _service.CheckHealthAsync(_context);
            var reportToJson = report.ToJson();

            _logger.LogInformation($"Get Health Information: {reportToJson}");

            return report.Status == HealthStatus.Healthy ? Ok(reportToJson) : StatusCode((int)HttpStatusCode.ServiceUnavailable, reportToJson);
        }
    }
}

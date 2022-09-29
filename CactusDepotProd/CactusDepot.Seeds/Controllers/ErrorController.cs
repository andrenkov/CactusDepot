using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SeedsCatalogSrv.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> _logger)
        {
            logger = _logger;
        }

        [AllowAnonymous]
        [Route("Error/{statuscode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            if (HttpContext is not null)
            {
                IStatusCodeReExecuteFeature? statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                switch (statusCode)
                {
                    case 404:
                        ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                        if (statusCodeResult is not null)
                        {
                            ViewBag.Path = statusCodeResult.OriginalPath;
                            ViewBag.QS = statusCodeResult.OriginalQueryString;
                        }
                        break;
                }
            }

            return View("NotFound");
        }

        //to work with unhanded errors 
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            if (HttpContext is not null)
            {
                IExceptionHandlerPathFeature? exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionDetails is not null)
                {
                    logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error.Message}");
                    ViewBag.ExceptionPath = exceptionDetails.Path;
                    ViewBag.ErrorMessage = exceptionDetails.Error.Message;
                    ViewBag.StackTrace = exceptionDetails.Error.StackTrace;
                }
            }

            return View("Error");
        }
    }
}

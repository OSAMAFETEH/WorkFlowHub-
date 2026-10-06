using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
namespace WorkFlowHub.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
      
        public IActionResult Error()
        {
            var exception=HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            if(exception is not null )
            {
                _logger.LogError(exception, "An unhandled exception occurred.");
            }
            return View();
        }
       
    }
}

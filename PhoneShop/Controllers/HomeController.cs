using System.Diagnostics;
using System.Threading;
using Application.DTO.Frontend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PhoneShop.Controllers
{
    [Route("home")]
    [Route("")]
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error(CancellationToken token)
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet("index")]
        [HttpGet("")]
        public ActionResult Index(CancellationToken token)
        {
            return View();
        }
    }
}
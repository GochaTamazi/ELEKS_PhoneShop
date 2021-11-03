using Application.DTO.Frontend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading;

namespace PhoneShop.Controllers
{
    [AllowAnonymous]
    [Route("home"), Route("")]
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

        [HttpGet("index"), HttpGet("")]
        public ActionResult Index(CancellationToken token)
        {
            return View();
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Content("Not authenticated");
            }

            var role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
            var role1 = User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType);
            var str =
                $"{User.Identity?.Name} | " +
                $"{User.Identity?.AuthenticationType} | " +
                $"{User.Identity?.IsAuthenticated} | {role} {role1}";
            return Content(str);
        }
    }
}
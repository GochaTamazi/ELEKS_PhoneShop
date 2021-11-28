using Application.DTO.Frontend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PhoneShop.Controllers
{
    [AllowAnonymous]
    [Route("Home"), Route("")]
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet("Index"), HttpGet("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PhoneShop.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet("index")]
        public async Task<ActionResult> Index(CancellationToken token)
        {
            return Ok();
        }
    }
}
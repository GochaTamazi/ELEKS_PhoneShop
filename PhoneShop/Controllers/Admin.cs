using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PhoneShop.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private ISynchronizeDb Synchronize { get; set; }

        public AdminController(ISynchronizeDb iSynchronizeDb)
        {
            Synchronize = iSynchronizeDb;
        }

        [HttpGet("index")]
        [HttpGet("")]
        public ActionResult Index(CancellationToken token)
        {
            return View();
        }

        [HttpGet("SynchronizeBrands")]
        public async Task<ActionResult> SynchronizeBrandsAsync(CancellationToken token)
        {
            await Synchronize.BrandsAsync(token);
            return Ok("Done");
        }

        [HttpGet("SynchronizePhones")]
        public async Task<ActionResult> SynchronizePhonesAsync(CancellationToken token)
        {
            await Synchronize.PhonesAsync(token);
            return Ok("Done");
        }

        [HttpGet("SynchronizeSpecifications")]
        public ActionResult SynchronizeSpecificationsAsync(CancellationToken token)
        {
            //await Synchronize.SpecificationsAsync(token);
            return Ok("Done");
        }
    }
}
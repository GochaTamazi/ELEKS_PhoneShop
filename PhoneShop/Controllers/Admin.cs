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
        public AdminController(ISynchronizeDb iSynchronizeDb)
        {
            Synchronize = iSynchronizeDb;
        }

        private ISynchronizeDb Synchronize { get; set; }

        [HttpGet("index")]
        public async Task<ActionResult> Index(CancellationToken token)
        {
            return Ok();
        }

        [HttpGet("synchronize")]
        public async Task<ActionResult> SynchronizeDb(CancellationToken token)
        {
            await Synchronize.BrandsAsync(token);

            //var cnt = brands.Sum(brand => brand.Device_count);
            //var phones = await Synchronize.PhonesAsync(brands, token);
            //var phonesDetails = await Synchronize.SpecificationsAsync(phones, token);


            /*var str =
                $"Brands.Count = {brands.Count}; " +
                $"Total Device_count = {cnt}" +
                $"Phones.Count = {phones.Count}"; // +*/
            //$"PhonesDetails.Count = {phonesDetails.Count}";

            var str = "sdfsdfds";

            return Ok(str);
        }
    }
}
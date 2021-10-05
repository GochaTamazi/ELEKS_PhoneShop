using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Models.DTO.RemoteAPI.Latest;
using Models.DTO.RemoteAPI.ListBrands;
using Models.DTO.RemoteAPI.ListPhones;
using Models.DTO.RemoteAPI.PhoneSpecifications;
using Models.DTO.RemoteAPI.Search;
using Models.DTO.RemoteAPI.TopByFans;
using Models.DTO.RemoteAPI.TopByInterest;
using PhoneShop.DTO;

namespace PhoneShop.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private IPhoneSpecificationClient PhoneSpecification { get; set; }

        public ApiController(IPhoneSpecificationClient phoneSpecificationClient)
        {
            PhoneSpecification = phoneSpecificationClient;
        }

        [HttpGet("index")]
        [HttpGet("")]
        public ActionResult Index(CancellationToken token)
        {
            return View();
        }

        [HttpGet("listBrands")]
        public async Task<ActionResult<ListBrands>> ListBrandsAsync(CancellationToken token)
        {
            var listBrands = await PhoneSpecification.ListBrandsAsync(token);
            return View(listBrands);
        }

        [HttpGet("listPhones")]
        public async Task<ActionResult<ListPhones>> ListPhonesAsync([FromQuery] string brandSlug,
            [FromQuery] int page,
            CancellationToken token)
        {
            var listPhonesRes = new ListPhonesRes()
            {
                Phones = await PhoneSpecification.ListPhonesAsync(brandSlug, page, token),
                BrandSlug = brandSlug,
                Page = page
            };
            return View(listPhonesRes);
        }

        [HttpGet("phoneSpecifications")]
        public async Task<ActionResult<PhoneSpecifications>> PhoneSpecificationsAsync([FromQuery] string phoneSlug,
            CancellationToken token)
        {
            var phoneSpecifications = await PhoneSpecification.PhoneSpecificationsAsync(phoneSlug, token);
            return View(phoneSpecifications);
        }

        [HttpGet("search")]
        public async Task<ActionResult<Search>> SearchAsync([FromQuery] string query, CancellationToken token)
        {
            var search = await PhoneSpecification.SearchAsync(query, token);
            return View(search);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<Latest>> LatestAsync(CancellationToken token)
        {
            var latest = await PhoneSpecification.LatestAsync(token);
            return View(latest);
        }

        [HttpGet("topByInterest")]
        public async Task<ActionResult<TopByInterest>> TopByInterestAsync(CancellationToken token)
        {
            var topByInterest = await PhoneSpecification.TopByInterestAsync(token);
            return View(topByInterest);
        }

        [HttpGet("topByFans")]
        public async Task<ActionResult<TopByFans>> TopByFansAsync(CancellationToken token)
        {
            var topByFans = await PhoneSpecification.TopByFansAsync(token);
            return View(topByFans);
        }
    }
}
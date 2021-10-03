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
        private readonly IPhoneSpecificationClient _phoneSpecification;

        public ApiController(IPhoneSpecificationClient phoneSpecificationClient)
        {
            _phoneSpecification = phoneSpecificationClient;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("listBrands")]
        public async Task<ActionResult<ListBrands>> ListBrandsAsync(CancellationToken token)
        {
            var listBrands = await _phoneSpecification.ListBrandsAsync(token);
            return View(listBrands);
        }

        [HttpGet("listPhones")]
        public async Task<ActionResult<ListPhones>> ListPhonesAsync([FromQuery] string brandSlug,
            [FromQuery] int page,
            CancellationToken token)
        {
            var listPhonesRes = new ListPhonesRes()
            {
                Phones = await _phoneSpecification.ListPhonesAsync(brandSlug, page, token),
                BrandSlug = brandSlug,
                Page = page
            };
            return View(listPhonesRes);
        }

        [HttpGet("phoneSpecifications")]
        public async Task<ActionResult<PhoneSpecifications>> PhoneSpecificationsAsync([FromQuery] string phoneSlug,
            CancellationToken token)
        {
            var phoneSpecifications = await _phoneSpecification.PhoneSpecificationsAsync(phoneSlug, token);
            return View(phoneSpecifications);
        }

        [HttpGet("search")]
        public async Task<ActionResult<Search>> SearchAsync([FromQuery] string query, CancellationToken token)
        {
            var search = await _phoneSpecification.SearchAsync(query, token);
            return View(search);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<Latest>> LatestAsync(CancellationToken token)
        {
            var latest = await _phoneSpecification.LatestAsync(token);
            return View(latest);
        }

        [HttpGet("topByInterest")]
        public async Task<ActionResult<TopByInterest>> TopByInterestAsync(CancellationToken token)
        {
            var topByInterest = await _phoneSpecification.TopByInterestAsync(token);
            return View(topByInterest);
        }

        [HttpGet("topByFans")]
        public async Task<ActionResult<TopByFans>> TopByFansAsync(CancellationToken token)
        {
            var topByFans = await _phoneSpecification.TopByFansAsync(token);
            return View(topByFans);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend;
using Application.DTO.PhoneSpecificationsAPI.Latest;
using Application.DTO.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.PhoneSpecificationsAPI.ListPhones;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using Application.DTO.PhoneSpecificationsAPI.Search;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;
using Application.DTO.PhoneSpecificationsAPI.TopByInterest;
using Application.Interfaces;

namespace PhoneShop.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IPhoneSpecificationsApi _phoneSpecificationServiceApi;
        private readonly IAdminPhones _adminPhones;

        public AdminController(
            IPhoneSpecificationsApi phoneSpecificationsServiceApiApi,
            IAdminPhones adminPhones
        )
        {
            _phoneSpecificationServiceApi = phoneSpecificationsServiceApiApi;
            _adminPhones = adminPhones;
        }

        [HttpGet("index"), HttpGet("")]
        public ActionResult Index(CancellationToken token)
        {
            return View();
        }

        [HttpGet("listBrands")]
        public async Task<ActionResult<ListBrandsDto>> ListBrandsAsync(CancellationToken token)
        {
            var listBrands = await _phoneSpecificationServiceApi.ListBrandsAsync(token);
            return View(listBrands);
        }

        [HttpGet("listPhones")]
        public async Task<ActionResult<ListPhonesDto>> ListPhonesAsync(
            [FromQuery] string brandSlug,
            [FromQuery] int page,
            CancellationToken token
        )
        {
            var listPhonesRes = new ListPhonesFront()
            {
                Phones = await _phoneSpecificationServiceApi.ListPhonesAsync(brandSlug, page, token),
                BrandSlug = brandSlug,
                Page = page
            };
            return View(listPhonesRes);
        }

        [HttpGet("getPhonesInStore")]
        public async Task<ActionResult<PhonesPageFront>> GetPhonesInStoreAsync(
            CancellationToken token,
            [FromQuery] int page = 1
        )
        {
            const int pageSize = 5;
            var phonesPageFront = await _adminPhones.GetPhonesInStoreAsync(page, pageSize, token);
            return View(phonesPageFront);
        }

        [HttpGet("phoneSpecifications")]
        public async Task<ActionResult<PhoneSpecificationsDto>> PhoneSpecificationsAsync(
            [FromQuery] string phoneSlug,
            CancellationToken token
        )
        {
            var phoneSpecFront = await _adminPhones.GetPhone(phoneSlug, token);
            return View(phoneSpecFront);
        }

        [HttpPost("phoneInsertOrUpdate")]
        public async Task<ActionResult<string>> PhoneInsertOrUpdateAsync(
            [FromForm] PhoneSpecFront phoneSpecFront,
            CancellationToken token
        )
        {
            await _adminPhones.PhoneInsertOrUpdateAsync(phoneSpecFront, token);
            return Ok("PhoneInsertOrUpdateAsync Done");
        }

        [HttpGet("search")]
        public async Task<ActionResult<SearchDto>> SearchAsync(
            [FromQuery] string query,
            CancellationToken token
        )
        {
            var search = await _phoneSpecificationServiceApi.SearchAsync(query, token);
            return View(search);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<LatestDto>> LatestAsync(CancellationToken token)
        {
            var latest = await _phoneSpecificationServiceApi.LatestAsync(token);
            return View(latest);
        }

        [HttpGet("topByInterest")]
        public async Task<ActionResult<TopByInterestDto>> TopByInterestAsync(CancellationToken token)
        {
            var topByInterest = await _phoneSpecificationServiceApi.TopByInterestAsync(token);
            return View(topByInterest);
        }

        [HttpGet("topByFans")]
        public async Task<ActionResult<TopByFansDto>> TopByFansAsync(CancellationToken token)
        {
            var topByFans = await _phoneSpecificationServiceApi.TopByFansAsync(token);
            return View(topByFans);
        }
    }
}
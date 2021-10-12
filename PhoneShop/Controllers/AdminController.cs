using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.Latest;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.ListPhones;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.Search;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.TopByFans;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.TopByInterest;
using Application.Interfaces;
using Application.Interfaces.RemoteAPI;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.PhoneSpecifications;

namespace PhoneShop.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IPhoneSpecificationsApi _phoneSpecification;
        private readonly IAdminPhones _adminPhones;

        public AdminController(IPhoneSpecificationsApi phoneSpecificationsApi, IAdminPhones adminPhones)
        {
            _phoneSpecification = phoneSpecificationsApi;
            _adminPhones = adminPhones;
        }

        [HttpGet("index")]
        [HttpGet("")]
        public ActionResult Index(CancellationToken token)
        {
            return View();
        }

        [HttpGet("listBrands")]
        public async Task<ActionResult<ListBrandsDto>> ListBrandsAsync(CancellationToken token)
        {
            var listBrands = await _phoneSpecification.ListBrandsAsync(token);
            return View(listBrands);
        }

        [HttpGet("listPhones")]
        public async Task<ActionResult<ListPhonesDto>> ListPhonesAsync([FromQuery] string brandSlug,
            [FromQuery] int page,
            CancellationToken token)
        {
            var listPhonesRes = new ListPhonesFront()
            {
                Phones = await _phoneSpecification.ListPhonesAsync(brandSlug, page, token),
                BrandSlug = brandSlug,
                Page = page
            };
            return View(listPhonesRes);
        }

        [HttpGet("phoneSpecifications")]
        public async Task<ActionResult<PhoneSpecificationsDto>> PhoneSpecificationsAsync([FromQuery] string phoneSlug,
            CancellationToken token)
        {
            var phone = new PhoneSpecFront()
            {
                PhoneSlug = phoneSlug,
                Specification = await _phoneSpecification.PhoneSpecificationsAsync(phoneSlug, token)
            };
            return View(phone);
        }

        [HttpGet("search")]
        public async Task<ActionResult<SearchDto>> SearchAsync([FromQuery] string query, CancellationToken token)
        {
            var search = await _phoneSpecification.SearchAsync(query, token);
            return View(search);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<LatestDto>> LatestAsync(CancellationToken token)
        {
            var latest = await _phoneSpecification.LatestAsync(token);
            return View(latest);
        }

        [HttpGet("topByInterest")]
        public async Task<ActionResult<TopByInterestDto>> TopByInterestAsync(CancellationToken token)
        {
            var topByInterest = await _phoneSpecification.TopByInterestAsync(token);
            return View(topByInterest);
        }

        [HttpGet("topByFans")]
        public async Task<ActionResult<TopByFansDto>> TopByFansAsync(CancellationToken token)
        {
            var topByFans = await _phoneSpecification.TopByFansAsync(token);
            return View(topByFans);
        }
    }
}
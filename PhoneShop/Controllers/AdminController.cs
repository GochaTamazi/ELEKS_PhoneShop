using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.DTO.PhoneSpecificationsAPI.Latest;
using Application.DTO.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.PhoneSpecificationsAPI.ListPhones;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using Application.DTO.PhoneSpecificationsAPI.Search;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;
using Application.DTO.PhoneSpecificationsAPI.TopByInterest;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace PhoneShop.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IAdminPhones _adminPhones;
        private readonly IPhoneSpecificationsApi _phoneSpecificationServiceApi;

        public AdminController(
            IAdminPhones adminPhones,
            IPhoneSpecificationsApi phoneSpecificationsServiceApiApi
        )
        {
            _adminPhones = adminPhones;
            _phoneSpecificationServiceApi = phoneSpecificationsServiceApiApi;
        }

        [HttpGet("index"), HttpGet("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("listBrands")]
        public async Task<ActionResult<ListBrandsDto>> ListBrandsAsync(CancellationToken token)
        {
            var listBrands = await _phoneSpecificationServiceApi.GetListBrandsAsync(token);
            return View(listBrands);
        }

        [HttpGet("listPhones")]
        public async Task<ActionResult<ListPhonesDto>> ListPhonesAsync([FromQuery] string brandSlug,
            [FromQuery] int page, CancellationToken token)
        {
            var listPhonesRes = new ListPhonesFront()
            {
                Phones = await _phoneSpecificationServiceApi.GetListPhonesAsync(brandSlug, page, token),
                BrandSlug = brandSlug,
                Page = page
            };
            return View(listPhonesRes);
        }

        [HttpGet("showAllPhones")]
        public async Task<ActionResult<PhonesPageFront>> ShowPhonesAsync(CancellationToken token,
            [FromQuery] PhonesFilterForm filterForm, [FromQuery] int page = 1)
        {
            const int pageSize = 10;
            var phonesPageFront = await _adminPhones.GetPhonesAsync(filterForm, page, pageSize, token);
            phonesPageFront.FilterForm = filterForm;
            return View(phonesPageFront);
        }

        [HttpGet("phoneSpecifications")]
        public async Task<ActionResult<PhoneSpecificationsDto>> PhoneSpecificationsAsync(
            [FromQuery(Name = "phoneSlug")] [Required] string phoneSlug,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var phoneSpecFront = await _adminPhones.GetPhoneAsync(phoneSlug, token);
            return View(phoneSpecFront);
        }

        [HttpPost("phoneInsertOrUpdate")]
        public async Task<ActionResult<string>> PhoneInsertOrUpdateAsync([FromForm] PhoneSpecFront phoneSpecFront,
            CancellationToken token)
        {
            await _adminPhones.PhoneInsertOrUpdateAsync(phoneSpecFront, token);
            return Ok("PhoneInsertOrUpdateAsync Done");
        }

        [HttpGet("search")]
        public async Task<ActionResult<SearchDto>> SearchAsync([FromQuery] string query, CancellationToken token)
        {
            var search = await _phoneSpecificationServiceApi.SearchAsync(query, token);
            return View(search);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<LatestDto>> LatestAsync(CancellationToken token)
        {
            var latest = await _phoneSpecificationServiceApi.GetLatestAsync(token);
            return View(latest);
        }

        [HttpGet("topByInterest")]
        public async Task<ActionResult<TopByInterestDto>> TopByInterestAsync(CancellationToken token)
        {
            var topByInterest = await _phoneSpecificationServiceApi.GetTopByInterestAsync(token);
            return View(topByInterest);
        }

        [HttpGet("topByFans")]
        public async Task<ActionResult<TopByFansDto>> TopByFansAsync(CancellationToken token)
        {
            var topByFans = await _phoneSpecificationServiceApi.GetTopByFansAsync(token);
            return View(topByFans);
        }
    }
}
using Application.DTO.Frontend;
using Application.DTO.PhoneSpecificationsAPI.Latest;
using Application.DTO.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.PhoneSpecificationsAPI.ListPhones;
using Application.DTO.PhoneSpecificationsAPI.Search;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;
using Application.DTO.PhoneSpecificationsAPI.TopByInterest;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace PhoneShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("AdminPhonesApi")]
    public class AdminPhonesApiController : Controller
    {
        private readonly IPhoneSpecificationsApi _phoneSpecificationServiceApi;

        public AdminPhonesApiController(IPhoneSpecificationsApi phoneSpecificationsServiceApiApi)
        {
            _phoneSpecificationServiceApi = phoneSpecificationsServiceApiApi;
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<ListBrandsDto>> GetBrandsAsync(CancellationToken token)
        {
            var apiResponseDto = await _phoneSpecificationServiceApi.GetListBrandsAsync(token);
            if (apiResponseDto.StatusCode != HttpStatusCode.OK)
            {
                return View("ApiError", apiResponseDto);
            }

            var listBrands = (ListBrandsDto) apiResponseDto.Data;
            if (listBrands == null)
            {
                return BadRequest("Api not respond");
            }

            return View(listBrands);
        }

        [HttpGet("Phones")]
        public async Task<ActionResult<ListPhonesDto>> GetPhonesAsync(CancellationToken token,
            [FromQuery] [Required] string brandSlug,
            [FromQuery] int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("brandSlug not set");
            }

            var apiResponseDto = await _phoneSpecificationServiceApi.GetListPhonesAsync(brandSlug, page, token);
            if (apiResponseDto.StatusCode != HttpStatusCode.OK)
            {
                return View("ApiError", apiResponseDto);
            }

            var phones = (ListPhonesDto) apiResponseDto.Data;
            if (phones == null)
            {
                return BadRequest("Api not respond");
            }

            var listPhonesRes = new ListPhonesFront()
            {
                Phones = phones,
                BrandSlug = brandSlug,
                Page = page
            };
            return View(listPhonesRes);
        }

        [HttpGet("Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("Latest")]
        public async Task<ActionResult<LatestDto>> LatestAsync(CancellationToken token)
        {
            var apiResponseDto = await _phoneSpecificationServiceApi.GetLatestAsync(token);
            if (apiResponseDto.StatusCode != HttpStatusCode.OK)
            {
                return View("ApiError", apiResponseDto);
            }

            var latest = (LatestDto) apiResponseDto.Data;
            if (latest == null)
            {
                return BadRequest("Api not respond");
            }

            return View(latest);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<SearchDto>> SearchAsync(CancellationToken token,
            [FromQuery] [Required] string query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("query not set");
            }

            var apiResponseDto = await _phoneSpecificationServiceApi.SearchAsync(query, token);
            if (apiResponseDto.StatusCode != HttpStatusCode.OK)
            {
                return View("ApiError", apiResponseDto);
            }

            var search = (SearchDto) apiResponseDto.Data;
            if (search == null)
            {
                return BadRequest("Api not respond");
            }

            return View(search);
        }

        [HttpGet("TopByFans")]
        public async Task<ActionResult<TopByFansDto>> TopByFansAsync(CancellationToken token)
        {
            var apiResponseDto = await _phoneSpecificationServiceApi.GetTopByFansAsync(token);
            if (apiResponseDto.StatusCode != HttpStatusCode.OK)
            {
                return View("ApiError", apiResponseDto);
            }

            var topByFans = (TopByFansDto) apiResponseDto.Data;
            if (topByFans == null)
            {
                return BadRequest("Api not respond");
            }

            return View(topByFans);
        }

        [HttpGet("TopByInterest")]
        public async Task<ActionResult<TopByInterestDto>> TopByInterestAsync(CancellationToken token)
        {
            var apiResponseDto = await _phoneSpecificationServiceApi.GetTopByInterestAsync(token);
            if (apiResponseDto.StatusCode != HttpStatusCode.OK)
            {
                return View("ApiError", apiResponseDto);
            }

            var topByInterest = (TopByInterestDto) apiResponseDto.Data;
            if (topByInterest == null)
            {
                return BadRequest("Api not respond");
            }

            return View(topByInterest);
        }
    }
}
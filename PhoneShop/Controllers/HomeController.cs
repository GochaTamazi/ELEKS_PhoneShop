using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTO.RemoteAPI.Latest;
using Models.DTO.RemoteAPI.ListBrands;
using Models.DTO.RemoteAPI.ListPhones;
using Models.DTO.RemoteAPI.PhoneSpecifications;
using Models.DTO.RemoteAPI.Search;
using Models.DTO.RemoteAPI.TopByFans;
using Models.DTO.RemoteAPI.TopByInterest;
using Models.Entities;
using PhoneShop.DTO;

namespace PhoneShop.Controllers
{
    [Route("home")]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhoneSpecificationClient _phoneSpecification;

        private readonly ITestService _testService;

        public HomeController(ILogger<HomeController> logger, IPhoneSpecificationClient phoneSpecificationClient,
            ITestService testService)
        {
            _logger = logger;
            _phoneSpecification = phoneSpecificationClient;
            _testService = testService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet("index")]
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("privacy")]
        public async Task<IActionResult> Privacy(CancellationToken token)
        {
            //await _testService.RunTest(ct);
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
        public async Task<ActionResult<PhoneSpecifications>> PhoneSpecificationsAsync(string phoneSlug,
            CancellationToken token)
        {
            var phoneSpecifications = await _phoneSpecification.PhoneSpecificationsAsync(phoneSlug, token);
            return View(phoneSpecifications);
        }

        [HttpGet("search")]
        public async Task<ActionResult<Search>> SearchAsync(CancellationToken token)
        {
            var search = await _phoneSpecification.SearchAsync("iPhone 12 pro max", token);
            return Ok(search);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<Latest>> LatestAsync(CancellationToken token)
        {
            var latest = await _phoneSpecification.LatestAsync(token);
            return Ok(latest);
        }

        [HttpGet("topByInterest")]
        public async Task<ActionResult<TopByInterest>> TopByInterestAsync(CancellationToken token)
        {
            var topByInterest = await _phoneSpecification.TopByInterestAsync(token);
            return Ok(topByInterest);
        }

        [HttpGet("topByFans")]
        public async Task<ActionResult<TopByFans>> TopByFansAsync(CancellationToken token)
        {
            var topByFans = await _phoneSpecification.TopByFansAsync(token);
            return Ok(topByFans);
        }
    }
}
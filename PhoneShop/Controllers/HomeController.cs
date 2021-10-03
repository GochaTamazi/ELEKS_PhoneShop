using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Entities;

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
            await _testService.RunTest(token);
            return View();
        }
    }
}
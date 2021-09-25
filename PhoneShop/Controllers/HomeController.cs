using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhoneShop.Models;
using PhoneShop.RemoteAPI;

namespace PhoneShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IPhoneSpecification _phoneSpecification;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _phoneSpecification = new PhoneSpecification();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult<string>> ListBrands()
        {
            var json = await _phoneSpecification.ListBrandsAsync();

            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            var jsonPretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

            return Ok(jsonPretty);
        }

        public async Task<ActionResult<string>> ListPhones()
        {
            var json = await _phoneSpecification.ListPhonesAsync("acer-phones-59");

            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            var jsonPretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

            return Ok(jsonPretty);
        }

        public async Task<ActionResult<string>> PhoneSpecifications()
        {
            var json = await _phoneSpecification.PhoneSpecificationsAsync("acer_chromebook_tab_10-9139");

            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            var jsonPretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

            return Ok(jsonPretty);
        }

        public async Task<ActionResult<string>> Search()
        {
            var json = await _phoneSpecification.SearchAsync("iPhone 12 pro max");

            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            var jsonPretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

            return Ok(jsonPretty);
        }

        public async Task<ActionResult<string>> Latest()
        {
            var json = await _phoneSpecification.LatestAsync();

            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            var jsonPretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

            return Ok(jsonPretty);
        }

        public async Task<ActionResult<string>> TopByInterest()
        {
            var json = await _phoneSpecification.TopByInterestAsync();

            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            var jsonPretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

            return Ok(jsonPretty);
        }

        public async Task<ActionResult<string>> TopByFans()
        {
            var json = await _phoneSpecification.TopByFansAsync();

            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            var jsonPretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

            return Ok(jsonPretty);
        }
    }
}
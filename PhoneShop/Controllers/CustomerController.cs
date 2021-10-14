using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend;
using Application.Interfaces;

namespace PhoneShop.Controllers
{
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerPhones _customerPhones;

        public CustomerController(ICustomerPhones customerPhones)
        {
            _customerPhones = customerPhones;
        }

        [HttpGet("index")]
        [HttpGet("")]
        public async Task<ActionResult> IndexAsync(CancellationToken token)
        {
            return View();
        }

        [HttpGet("showAllPhones")]
        public async Task<ActionResult> ShowPhonesAsync(CancellationToken token,
            [FromQuery] string brandName = "",
            [FromQuery] string phoneName = "",
            [FromQuery] uint priceMin = 0,
            [FromQuery] uint priceMax = 10_000,
            [FromQuery] bool inStock = true)
        {
            var filter = new PhonesFilter()
            {
                BrandName = brandName,
                PhoneName = phoneName,
                PriceMin = priceMin,
                PriceMax = priceMax,
                InStock = inStock
            };

            var phones = await _customerPhones.GetPhonesAsync(filter, token);

            return View(phones);
        }

        [HttpGet("showPhone")]
        public async Task<ActionResult> ShowPhoneAsync([FromQuery] string phoneSlug, CancellationToken token)
        {
            var phone = await _customerPhones.GetPhoneAsync(phoneSlug, token);
            return View(phone);
        }
    }
}
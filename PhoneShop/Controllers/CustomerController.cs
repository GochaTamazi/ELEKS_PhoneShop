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

        [HttpGet("index"), HttpGet("")]
        public ActionResult IndexAsync(CancellationToken token)
        {
            return View();
        }

        [HttpGet("showAllPhones")]
        public async Task<ActionResult<PhonesPageFront>> ShowPhonesAsync(CancellationToken token,
            [FromQuery] string brandName = "",
            [FromQuery] string phoneName = "",
            [FromQuery] uint priceMin = 0,
            [FromQuery] uint priceMax = 10_000,
            [FromQuery] bool inStock = true,
            [FromQuery] int page = 1
        )
        {
            var filter = new PhonesFilter()
            {
                BrandName = brandName,
                PhoneName = phoneName,
                PriceMin = priceMin,
                PriceMax = priceMax,
                InStock = inStock
            };

            const int pageSize = 10;
            var phonesPageFront = await _customerPhones.GetPhonesAsync(filter, page, pageSize, token);
            phonesPageFront.Filter = filter;
            return View(phonesPageFront);
        }

        [HttpGet("showPhone")]
        public async Task<ActionResult> ShowPhoneAsync(
            [FromQuery] string phoneSlug,
            CancellationToken token
        )
        {
            var phone = await _customerPhones.GetPhoneAsync(phoneSlug, token);
            return View(phone);
        }

        [HttpPost("subscribePrice")]
        public async Task<ActionResult> SubscribePriceAsync(
            [FromForm] PriceSubscriberFront priceSubscriber,
            CancellationToken token
        )
        {
            await _customerPhones.SubscribePriceAsync(priceSubscriber, token);
            return Ok("SubscribePriceAsync ok");
        }

        [HttpPost("subscribeStock")]
        public async Task<ActionResult> SubscribeStockAsync(
            [FromForm] StockSubscriberFront stockSubscriber,
            CancellationToken token
        )
        {
            await _customerPhones.SubscribeStockAsync(stockSubscriber, token);
            return Ok("SubscribeStockAsync ok");
        }
    }
}
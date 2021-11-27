using Application.DTO.Frontend.Forms;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace PhoneShop.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    [Route("CustomerSubscriber")]
    public class CustomerSubscriberController : Controller
    {
        private readonly ISubscribers _subscribers;

        public CustomerSubscriberController(ISubscribers subscribers)
        {
            _subscribers = subscribers;
        }

        [HttpPost("phone/subscribePrice")]
        public async Task<ActionResult> SubscribePriceAsync(CancellationToken token,
            [FromForm] SubscriberForm subscriberForm)
        {
            await _subscribers.SubscribeOnPriceAsync(subscriberForm, token);
            return Ok("SubscribePriceAsync ok");
        }

        [HttpPost("phone/subscribeStock")]
        public async Task<ActionResult> SubscribeStockAsync(CancellationToken token,
            [FromForm] SubscriberForm subscriberForm)
        {
            await _subscribers.SubscribeOnStockAsync(subscriberForm, token);
            return Ok("SubscribeStockAsync ok");
        }
    }
}
using System.ComponentModel.DataAnnotations;
using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace PhoneShop.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerPhones _customerPhones;
        private readonly ICustomerWishList _customerWishList;
        private readonly ISubscribers _subscribers;
        private readonly ICustomerComments _customerComments;
        private readonly ICustomerCart _customerCart;

        public CustomerController(
            ICustomerPhones customerPhones,
            ICustomerWishList customerWishList,
            ISubscribers subscribers,
            ICustomerComments customerComments,
            ICustomerCart customerCart
        )
        {
            _customerPhones = customerPhones;
            _customerWishList = customerWishList;
            _subscribers = subscribers;
            _customerComments = customerComments;
            _customerCart = customerCart;
        }

        [HttpGet("index"), HttpGet("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("showAllPhones")]
        public async Task<ActionResult<PhonesPageFront>> ShowPhonesAsync(CancellationToken token,
            [FromQuery] PhonesFilterForm filterForm, [FromQuery] int page = 1)
        {
            const int pageSize = 10;

            var phonesPageFront = await _customerPhones.GetAllAsync(filterForm, page, pageSize, token);
            phonesPageFront.FilterForm = filterForm;

            return View(phonesPageFront);
        }

        [HttpGet("showPhone")]
        public async Task<ActionResult> ShowPhoneAsync([FromQuery] [Required] string phoneSlug, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var phone = await _customerPhones.GetOneAsync(phoneSlug, token);

            if (phone == null)
            {
                return NoContent();
            }

            return View(phone);
        }

        [HttpGet("AddToWishList")]
        public async Task<ActionResult> AddToWishListAsync([FromQuery] [Required] string phoneSlug,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var userMail = User.Identity?.Name;
            await _customerWishList.InsertAsync(phoneSlug, userMail, token);
            return Ok("AddToWishListAsync ok");
        }

        [HttpGet("RemoveFromWishList")]
        public async Task<ActionResult> RemoveFromWishListAsync([FromQuery] [Required] string phoneSlug,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var userMail = User.Identity?.Name;
            await _customerWishList.DeleteAsync(phoneSlug, userMail, token);
            return Ok("RemoveFromWishListAsync ok");
        }

        [HttpGet("ShowWishList")]
        public async Task<ActionResult> ShowWishListAsync(CancellationToken token)
        {
            var userMail = User.Identity?.Name;
            var wishList = await _customerWishList.GetAllAsync(userMail, token);
            return View(wishList);
        }

        [HttpGet("showPhoneComments")]
        public async Task<ActionResult> ShowPhoneCommentsAsync(CancellationToken token,
            [FromQuery] [Required] string phoneSlug,
            [FromQuery] int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            const int pageSize = 10;
            var commentsPage = await _customerComments.GetAllAsync(phoneSlug, page, pageSize, token);
            return PartialView(commentsPage);
        }

        [HttpPost("postComment")]
        public async Task<ActionResult> PostComment([FromForm] CommentForm commentForm, CancellationToken token)
        {
            commentForm.UserMail = User.Identity?.Name;
            var result = await _customerComments.InsertAsync(commentForm, token);
            if (result)
            {
                return RedirectToAction("ShowPhone", "Customer", new
                {
                    phoneSlug = commentForm.PhoneSlug
                });
            }

            return BadRequest("Error PostComment");
        }

        [HttpPost("subscribePrice")]
        public async Task<ActionResult> SubscribePriceAsync([FromForm] PriceSubscriberForm priceSubscriber,
            CancellationToken token)
        {
            await _subscribers.SubscribeOnPriceAsync(priceSubscriber, token);
            return Ok("SubscribePriceAsync ok");
        }

        [HttpPost("subscribeStock")]
        public async Task<ActionResult> SubscribeStockAsync([FromForm] StockSubscriberForm stockSubscriber,
            CancellationToken token)
        {
            await _subscribers.SubscribeOnStockAsync(stockSubscriber, token);
            return Ok("SubscribeStockAsync ok");
        }

        [HttpGet("insertToCart")]
        public async Task<ActionResult> InsertToCartAsync([FromQuery] [Required] string phoneSlug,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var userMail = User.Identity?.Name;
            await _customerCart.InsertAsync(phoneSlug, userMail, token);
            return Ok("InsertToCartAsync ok");
        }

        [HttpGet("deleteFromCart")]
        public async Task<ActionResult> DeleteFromCartAsync([FromQuery] [Required] string phoneSlug,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var userMail = User.Identity?.Name;
            await _customerCart.DeleteAsync(phoneSlug, userMail, token);
            return Ok("DeleteFromCartAsync ok");
        }
    }
}
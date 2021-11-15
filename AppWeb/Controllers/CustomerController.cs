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

        [HttpGet("getPhones")]
        public async Task<ActionResult<PhonesPageFront>> GetPhonesAsync(CancellationToken token,
            [FromQuery] PhonesFilterForm filterForm,
            [FromQuery] int page = 1)
        {
            const int pageSize = 10;

            var phonesPageFront = await _customerPhones.GetAllAsync(filterForm, page, pageSize, token);
            phonesPageFront.FilterForm = filterForm;

            return View(phonesPageFront);
        }

        [HttpGet("getPhone")]
        public async Task<ActionResult> GetPhoneAsync([FromQuery] [Required] string phoneSlug, CancellationToken token)
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

        [HttpGet("addToWishList")]
        public async Task<ActionResult> AddToWishListAsync([FromQuery] [Required] string phoneSlug,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var userMail = User.Identity?.Name;
            await _customerWishList.AddIfNotExistAsync(phoneSlug, userMail, token);
            return Ok("AddToWishListAsync ok");
        }

        [HttpGet("removeWishList")]
        public async Task<ActionResult> RemoveWishListAsync([FromQuery] [Required] string phoneSlug,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var userMail = User.Identity?.Name;
            await _customerWishList.RemoveAsync(phoneSlug, userMail, token);
            return Ok("RemoveFromWishListAsync ok");
        }

        [HttpGet("getWishLists")]
        public async Task<ActionResult> GetWishListsAsync(CancellationToken token)
        {
            var userMail = User.Identity?.Name;
            var wishList = await _customerWishList.GetAllAsync(userMail, token);
            return View(wishList);
        }

        [HttpGet("getPhoneComments")]
        public async Task<ActionResult> GetPhoneCommentsAsync(CancellationToken token,
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

        [HttpPost("addOrUpdateComment")]
        public async Task<ActionResult> AddOrUpdateCommentAsync([FromForm] CommentForm commentForm,
            CancellationToken token)
        {
            commentForm.UserMail = User.Identity?.Name;
            var result = await _customerComments.AddOrUpdateAsync(commentForm, token);
            if (result)
            {
                return RedirectToAction("GetPhone", "Customer", new
                {
                    phoneSlug = commentForm.PhoneSlug
                });
            }

            return BadRequest("Error PostComment");
        }

        [HttpPost("subscribeOnPrice")]
        public async Task<ActionResult> SubscribeOnPriceAsync([FromForm] PriceSubscriberForm priceSubscriber,
            CancellationToken token)
        {
            await _subscribers.SubscribeOnPriceAsync(priceSubscriber, token);
            return Ok("SubscribePriceAsync ok");
        }

        [HttpPost("subscribeOnStock")]
        public async Task<ActionResult> SubscribeOnStockAsync([FromForm] StockSubscriberForm stockSubscriber,
            CancellationToken token)
        {
            await _subscribers.SubscribeOnStockAsync(stockSubscriber, token);
            return Ok("SubscribeStockAsync ok");
        }

        [HttpGet("getCart")]
        public async Task<ActionResult> GetCartAsync(CancellationToken token)
        {
            var userMail = User.Identity?.Name;
            var cart = await _customerCart.GetAllAsync(userMail, token);
            return View(cart);
        }

        [HttpGet("addToCart")]
        public async Task<ActionResult> AddToCartAsync([FromQuery] [Required] string phoneSlug,
            [FromQuery] [Required] int amount,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug or amount not set");
            }

            var userMail = User.Identity?.Name;
            await _customerCart.AddOrUpdateAsync(phoneSlug, userMail, amount, token);
            return Ok("InsertToCartAsync ok");
        }

        [HttpGet("removeFromCart")]
        public async Task<ActionResult> RemoveFromCartAsync([FromQuery] [Required] string phoneSlug,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var userMail = User.Identity?.Name;
            await _customerCart.RemoveAsync(phoneSlug, userMail, token);
            return Ok("DeleteFromCartAsync ok");
        }
    }
}
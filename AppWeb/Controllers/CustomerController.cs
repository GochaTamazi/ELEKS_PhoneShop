using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Database.Models;

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
        private readonly IPromoCodes _promoCodes;

        public CustomerController(
            ICustomerPhones customerPhones,
            ICustomerWishList customerWishList,
            ISubscribers subscribers,
            ICustomerComments customerComments,
            ICustomerCart customerCart,
            IPromoCodes promoCodes
        )
        {
            _customerPhones = customerPhones;
            _customerWishList = customerWishList;
            _subscribers = subscribers;
            _customerComments = customerComments;
            _customerCart = customerCart;
            _promoCodes = promoCodes;
        }

        [HttpGet("index"), HttpGet("")]
        public ActionResult Index()
        {
            return View();
        }


        #region Phones

        [HttpGet("phones")]
        public async Task<ActionResult<PhonesPageFront>> GetPhonesAsync(CancellationToken token,
            [FromQuery] PhonesFilterForm filterForm,
            [FromQuery] int page = 1)
        {
            const int pageSize = 10;

            var phonesPageFront = await _customerPhones.GetAllAsync(filterForm, page, pageSize, token);
            phonesPageFront.FilterForm = filterForm;

            return View(phonesPageFront);
        }

        [HttpGet("phone/{phoneSlug}")]
        public async Task<ActionResult> GetPhoneAsync([FromRoute] [Required] string phoneSlug, CancellationToken token)
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

        [HttpGet("phoneComments/{phoneSlug}/{page}")]
        public async Task<ActionResult> GetPhoneCommentsAsync(CancellationToken token,
            [FromRoute] [Required] string phoneSlug,
            [FromRoute] int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            const int pageSize = 10;
            var commentsPage = await _customerComments.GetAllAsync(phoneSlug, page, pageSize, token);
            return PartialView(commentsPage);
        }

        [HttpPost("phoneComments")]
        public async Task<ActionResult> AddOrUpdatePhoneCommentAsync([FromForm] CommentForm commentForm,
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

        #endregion


        #region WishLists

        [HttpGet("wishLists")]
        public async Task<ActionResult> GetWishListsAsync(CancellationToken token)
        {
            var userMail = User.Identity?.Name;
            var wishList = await _customerWishList.GetAllAsync(userMail, token);
            return View(wishList);
        }

        [HttpGet("wishList/add/{phoneSlug}")]
        public async Task<ActionResult> AddWishListAsync([FromRoute] [Required] string phoneSlug,
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

        [HttpGet("wishList/remove/{phoneSlug}")]
        public async Task<ActionResult> RemoveWishListAsync([FromRoute] [Required] string phoneSlug,
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

        #endregion


        #region Cart

        [HttpGet("cart")]
        public async Task<ActionResult> GetCartAsync(CancellationToken token, [FromQuery] string promoCodeKey = "")
        {
            var userMail = User.Identity?.Name;
            var cartAndPromoCodeFront = new CartAndPromoCodeFront()
            {
                Cart = await _customerCart.GetAllAsync(userMail, token) ?? new List<Cart>(),
                PromoCode = await _promoCodes.GetOneAsync(promoCodeKey, token)
            };
            return View(cartAndPromoCodeFront);
        }

        [HttpPost("cart")]
        public async Task<ActionResult> AddCartAsync([FromForm] [Required] string phoneSlug,
            [FromForm] [Required] int amount,
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

        [HttpGet("cart/remove/{phoneSlug}")]
        public async Task<ActionResult> RemoveCartAsync([FromRoute] [Required] string phoneSlug,
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

        [HttpGet("cart/buy")]
        public async Task<ActionResult> BuyCartAsync(CancellationToken token, [FromQuery] string promoCodeKey = "")
        {
            var userMail = User.Identity?.Name;

            var carts = await _customerCart.BuyAsync(userMail, token);

            var totalSum = await _promoCodes.Buy(carts, promoCodeKey, token);

            return Ok($"BuyPhones OK. Total sum {totalSum}");
        }

        #endregion


        #region Subscriber

        [HttpPost("phone/subscribePrice")]
        public async Task<ActionResult> SubscribePriceAsync([FromForm] SubscriberForm subscriberForm,
            CancellationToken token)
        {
            await _subscribers.SubscribeOnPriceAsync(subscriberForm, token);
            return Ok("SubscribePriceAsync ok");
        }

        [HttpPost("phone/subscribeStock")]
        public async Task<ActionResult> SubscribeStockAsync([FromForm] SubscriberForm subscriberForm,
            CancellationToken token)
        {
            await _subscribers.SubscribeOnStockAsync(subscriberForm, token);
            return Ok("SubscribeStockAsync ok");
        }

        #endregion
    }
}
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

        public CustomerController(ICustomerPhones customerPhones)
        {
            _customerPhones = customerPhones;
        }

        [HttpGet("index"), HttpGet("")]
        public ActionResult Index(CancellationToken token)
        {
            return View();
        }

        [HttpGet("showAllPhones")]
        public async Task<ActionResult<PhonesPageFront>> ShowPhonesAsync(
            CancellationToken token,
            [FromQuery] PhonesFilterForm filterForm,
            [FromQuery] int page = 1
        )
        {
            const int pageSize = 10;
            var phonesPageFront = await _customerPhones.GetPhonesAsync(filterForm, page, pageSize, token);
            phonesPageFront.FilterForm = filterForm;
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

        [HttpGet("AddToWishList")]
        public async Task<ActionResult> AddToWishListAsync(
            [FromQuery] string phoneSlug,
            CancellationToken token
        )
        {
            var userMail = User.Identity?.Name;
            await _customerPhones.AddToWishListAsync(phoneSlug, userMail, token);
            return Ok("AddToWishListAsync ok");
        }

        [HttpGet("RemoveFromWishList")]
        public async Task<ActionResult> RemoveFromWishListAsync(
            [FromQuery] string phoneSlug,
            CancellationToken token
        )
        {
            var userMail = User.Identity?.Name;
            await _customerPhones.RemoveFromWishListAsync(phoneSlug, userMail, token);
            return Ok("RemoveFromWishListAsync ok");
        }

        [HttpGet("ShowWishList")]
        public async Task<ActionResult> ShowWishListAsync(CancellationToken token)
        {
            var userMail = User.Identity?.Name;
            var wishList = await _customerPhones.ShowWishListAsync(userMail, token);
            return View(wishList);
        }

        [HttpGet("showPhoneComments")]
        public async Task<ActionResult> ShowPhoneCommentsAsync(
            CancellationToken token,
            [FromQuery] string phoneSlug,
            [FromQuery] int page = 1
        )
        {
            const int pageSize = 10;
            var commentsPage = await _customerPhones.GetPhoneCommentsAsync(phoneSlug, page, pageSize, token);
            return PartialView(commentsPage);
        }

        [HttpPost("postComment")]
        public async Task<ActionResult> PostComment(
            [FromForm] CommentForm commentForm,
            CancellationToken token
        )
        {
            commentForm.UserMail = User.Identity?.Name;
            var result = await _customerPhones.PostCommentAsync(commentForm, token);
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
        public async Task<ActionResult> SubscribePriceAsync(
            [FromForm] PriceSubscriberForm priceSubscriber,
            CancellationToken token
        )
        {
            await _customerPhones.SubscribePriceAsync(priceSubscriber, token);
            return Ok("SubscribePriceAsync ok");
        }

        [HttpPost("subscribeStock")]
        public async Task<ActionResult> SubscribeStockAsync(
            [FromForm] StockSubscriberForm stockSubscriber,
            CancellationToken token
        )
        {
            await _customerPhones.SubscribeStockAsync(stockSubscriber, token);
            return Ok("SubscribeStockAsync ok");
        }
    }
}
using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Threading;

namespace PhoneShop.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    [Route("CustomerPhonesShop")]
    public class CustomerPhonesShopController : Controller
    {
        private readonly ICustomerPhones _customerPhones;
        private readonly ICustomerComments _customerComments;

        public CustomerPhonesShopController(ICustomerPhones customerPhones, ICustomerComments customerComments)
        {
            _customerPhones = customerPhones;
            _customerComments = customerComments;
        }

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
        public async Task<ActionResult> GetPhoneAsync(CancellationToken token, [FromRoute] [Required] string phoneSlug)
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
        public async Task<ActionResult> AddOrUpdatePhoneCommentAsync(CancellationToken token,
            [FromForm] CommentForm commentForm)
        {
            commentForm.UserMail = User.Identity?.Name;
            var result = await _customerComments.AddOrUpdateAsync(commentForm, token);
            if (result)
            {
                return RedirectToAction("GetPhone", "CustomerPhonesShop", new
                {
                    phoneSlug = commentForm.PhoneSlug
                });
            }

            return BadRequest("Error PostComment");
        }
    }
}
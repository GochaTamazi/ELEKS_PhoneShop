using Application.DTO.Frontend.Forms;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Threading;

namespace PhoneShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("AdminPromoCodes")]
    public class AdminPromoCodesController : Controller
    {
        private readonly IPromoCodes _promoCodes;

        public AdminPromoCodesController(IPromoCodes promoCodes)
        {
            _promoCodes = promoCodes;
        }

        [HttpGet("PromoCodes")]
        public async Task<ActionResult<TopByFansDto>> GetPromoCodesAsync(CancellationToken token)
        {
            var listPromoCodes = await _promoCodes.GetAllAsync(token);
            return View(listPromoCodes);
        }

        [HttpPost("PromoCode")]
        public async Task<ActionResult<TopByFansDto>> CreatePromoCodeAsync(CancellationToken token,
            [FromForm] [Required] PromoCodeForm promoCodeForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not all fields are set");
            }

            await _promoCodes.AddOrUpdateAsync(promoCodeForm, token);
            return RedirectToAction("GetPromoCodes", "AdminPromoCodes");
        }

        [HttpGet("PromoCode/Remove")]
        public async Task<ActionResult<TopByFansDto>> RemoveCodeAsync(CancellationToken token,
            [FromQuery] [Required] string key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Key not set");
            }

            await _promoCodes.RemoveIfExistAsync(key, token);
            return RedirectToAction("GetPromoCodes", "AdminPromoCodes");
        }
    }
}
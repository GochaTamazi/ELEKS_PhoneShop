using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Net;

namespace PhoneShop.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("AdminPhonesShop")]
    public class AdminPhonesShopController : Controller
    {
        private readonly IAdminPhones _adminPhones;
        private readonly IPhoneData _phoneData;

        public AdminPhonesShopController(IAdminPhones adminPhones, IPhoneData phoneData)
        {
            _adminPhones = adminPhones;
            _phoneData = phoneData;
        }

        [HttpGet("Phones")]
        public async Task<ActionResult<PhonesPageFront>> GetPhonesAsync(CancellationToken token,
            [FromQuery] PhonesFilterForm filterForm,
            [FromQuery] int page = 1)
        {
            const int pageSize = 10;
            var phonesPageFront = await _adminPhones.GetAllPagedAsync(filterForm, page, pageSize, token);
            phonesPageFront.FilterForm = filterForm;
            return View(phonesPageFront);
        }

        [HttpGet("Phone/{phoneSlug}")]
        public async Task<ActionResult<PhoneSpecificationsDto>> GetPhoneAsync(CancellationToken token,
            [FromRoute] [Required] string phoneSlug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("phoneSlug not set");
            }

            var response = await _adminPhones.GetOneAsync(phoneSlug, token);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest($"ApiError: {response.Message} {response.StatusCode}");
            }

            var phoneSpecFront = (PhoneSpecFront) response.Data;
            if (phoneSpecFront == null)
            {
                return NoContent();
            }

            return View(phoneSpecFront);
        }

        [HttpPost("Phone")]
        public async Task<ActionResult<string>> AddOrUpdatePhoneAsync(CancellationToken token,
            [FromForm] PhoneSpecFront phoneSpecFront)
        {
            var phone = await _adminPhones.AddOrUpdateAsync(phoneSpecFront, token);
            if (phone == null)
            {
                return BadRequest("Api not respond");
            }

            return RedirectToAction("GetPhone", "AdminPhonesShop", new
            {
                phoneSlug = phone.PhoneSlug
            });
        }

        [HttpGet("ExportPhonesToExcel")]
        public async Task<ActionResult> ExportPhonesToExcelAsync(CancellationToken token,
            [FromQuery] PhonesFilterForm filterForm)
        {
            var phones = await _adminPhones.GetAllAsync(filterForm, token);
            var data = await _phoneData.ExportToXlsxAsync(phones, token);
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var str = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            var fileName = $"Phones_{str}.xlsx";
            return File(data, contentType, fileName);
        }

        [HttpPost("ImportPhonesFromExcel")]
        public async Task<ActionResult> ImportPhonesFromExcelAsync(CancellationToken token,
            [FromForm] IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var type = uploadedFile.FileName.Split(".").Last().ToLower();
                if (type == "xlsx")
                {
                    uploadedFile.OpenReadStream();
                    var phones = await _phoneData.ImportFromXlsxAsync(uploadedFile.OpenReadStream(), token);
                    await _adminPhones.AddOrUpdateAsync(phones, token);
                }
            }

            return RedirectToAction("Index", "AdminPhonesApi");
        }
    }
}
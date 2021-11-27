using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Frontend.Forms
{
    public class PromoCodeForm
    {
        [Required] public string PhoneSlug { set; get; }
        [Required] public string Key { set; get; }
        [Required] public int Amount { set; get; }
        [Required] public int Discount { set; get; }
    }
}
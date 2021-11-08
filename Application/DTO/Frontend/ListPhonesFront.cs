using Application.DTO.PhoneSpecificationsAPI.ListPhones;

namespace Application.DTO.Frontend
{
    public class ListPhonesFront
    {
        public ListPhonesDto Phones { set; get; } = new ListPhonesDto();
        public string BrandSlug { set; get; } = string.Empty;
        public int Page { set; get; } = 0;
    }
}
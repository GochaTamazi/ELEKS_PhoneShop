using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.ListPhones;

namespace Application.DTO.Frontend
{
    public class ListPhonesFront
    {
        public ListPhonesDto Phones { set; get; }
        public string BrandSlug { set; get; }
        public int Page { set; get; }
    }
}
using Models.DTO.RemoteAPI.ListPhones;

namespace PhoneShop.DTO
{
    public class ListPhonesRes
    {
        public ListPhones Phones { set; get; }
        public string BrandSlug { set; get; }
        public int Page { set; get; }
    }
}
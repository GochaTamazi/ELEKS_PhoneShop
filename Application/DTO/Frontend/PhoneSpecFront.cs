using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;

namespace Application.DTO.Frontend
{
    public class PhoneSpecFront
    {
        public PhoneDetailDto PhoneDetail { set; get; } = new PhoneDetailDto();
        public string BrandSlug { set; get; }
        public string PhoneSlug { set; get; }
        public bool InStore { set; get; } = false;
        public int? Price { set; get; } = 0;
        public int? Stock { set; get; } = 0;
        public bool Hided { set; get; } = false;
    }
}
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.PhoneSpecifications;

namespace Application.DTO.Frontend
{
    public class PhoneSpecFront
    {
        public string PhoneSlug { set; get; }
        public bool InStore { set; get; }
        public bool Hided { set; get; }
        public PhoneSpecificationsDto Specification { set; get; }
    }
}
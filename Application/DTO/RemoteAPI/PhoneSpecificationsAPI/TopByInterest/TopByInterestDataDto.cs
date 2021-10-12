using System.Collections.Generic;

namespace Application.DTO.RemoteAPI.PhoneSpecificationsAPI.TopByInterest
{
    public class TopByInterestDataDto
    {
        public string Title { get; set; }
        public ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
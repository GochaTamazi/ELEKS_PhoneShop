using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.TopByFans
{
    public class TopByFansDataDto
    {
        public string Title { get; set; }
        public ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
using System.Collections.Generic;

namespace Application.DTO.RemoteAPI.PhoneSpecificationsAPI.Latest
{
    public class LatestDataDto
    {
        public string Title { get; set; }
        public ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
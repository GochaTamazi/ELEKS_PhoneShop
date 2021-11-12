using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.Latest
{
    public class LatestDataDto
    {
        public string Title { get; set; } = string.Empty;
        public ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
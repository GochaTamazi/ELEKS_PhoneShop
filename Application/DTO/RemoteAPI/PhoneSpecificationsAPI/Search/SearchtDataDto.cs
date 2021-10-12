using System.Collections.Generic;

namespace Application.DTO.RemoteAPI.PhoneSpecificationsAPI.Search
{
    public class SearchtDataDto
    {
        public string Title { get; set; }
        public ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.Search
{
    public class SearchtDataDto
    {
        public string Title { get; set; } = string.Empty;
        public ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
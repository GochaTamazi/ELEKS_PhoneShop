using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications
{
    public class SpecificationDto
    {
        public string Title { get; set; } = string.Empty;
        public ICollection<KeyValDto> Specs { get; set; } = new List<KeyValDto>();
    }
}
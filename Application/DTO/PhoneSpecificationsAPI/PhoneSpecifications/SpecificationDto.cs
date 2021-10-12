using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications
{
    public class SpecificationDto
    {
        public string Title { get; set; }
        public ICollection<KeyValDto> Specs { get; set; } = new List<KeyValDto>();
    }
}
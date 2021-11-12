using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications
{
    public class PhoneDetailDto
    {
        public string Brand { get; set; } = string.Empty;
        public string Phone_name { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public ICollection<string> Phone_images { get; set; } = new List<string>();
        public string Release_date { get; set; } = string.Empty;
        public string Dimension { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        public string Storage { get; set; } = string.Empty;
        public ICollection<SpecificationDto> Specifications { get; set; } = new List<SpecificationDto>();
    }
}
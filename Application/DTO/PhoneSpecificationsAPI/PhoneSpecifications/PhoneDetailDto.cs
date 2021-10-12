using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications
{
    public class PhoneDetailDto
    {
        public string Brand { get; set; }
        public string Phone_name { get; set; }
        public string Thumbnail { get; set; }
        public ICollection<string> Phone_images { get; set; } = new List<string>();
        public string Release_date { get; set; }
        public string Dimension { get; set; }
        public string Os { get; set; }
        public string Storage { get; set; }
        public ICollection<SpecificationDto> Specifications { get; set; } = new List<SpecificationDto>();
    }
}
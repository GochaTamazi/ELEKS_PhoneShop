using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using System.Collections.Generic;

namespace Application.DTO.Frontend
{
    public class PhoneDto
    {
        public int Id { get; set; }
        public string BrandSlug { get; set; }
        public string PhoneSlug { get; set; }
        public string PhoneName { get; set; }
        public string Dimension { get; set; }
        public string Os { get; set; }
        public string Storage { get; set; }
        public string Thumbnail { get; set; }
        public string ReleaseDate { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public ICollection<SpecificationDto> Specifications { get; set; } = new List<SpecificationDto>();
        public int? Price { get; set; }
        public int? Stock { get; set; }
        public bool? Hided { get; set; }
        public double? AverageRating { get; set; } = 0;
    }
}
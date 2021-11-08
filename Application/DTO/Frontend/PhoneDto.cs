using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using System.Collections.Generic;

namespace Application.DTO.Frontend
{
    public class PhoneDto
    {
        public int Id { get; set; } = 0;
        public string BrandSlug { get; set; } = string.Empty;
        public string PhoneSlug { get; set; } = string.Empty;
        public string PhoneName { get; set; } = string.Empty;
        public string Dimension { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        public string Storage { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new List<string>();
        public ICollection<SpecificationDto> Specifications { get; set; } = new List<SpecificationDto>();
        public int? Price { get; set; } = 0;
        public int? Stock { get; set; } = 0;
        public bool? Hided { get; set; } = false;
        public double? AverageRating { get; set; } = 0;
    }
}
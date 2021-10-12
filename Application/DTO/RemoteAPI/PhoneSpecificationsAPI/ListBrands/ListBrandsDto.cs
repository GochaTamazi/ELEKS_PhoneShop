using System.Collections.Generic;

namespace Application.DTO.RemoteAPI.PhoneSpecificationsAPI.ListBrands
{
    public class ListBrandsDto
    {
        public bool Status { get; set; } = false;
        public ICollection<BrandDto> Data { get; set; } = new List<BrandDto>();
    }
}
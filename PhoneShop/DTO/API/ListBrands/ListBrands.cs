using System.Collections.Generic;

namespace PhoneShop.DTO.API.ListBrands
{
    public class ListBrands
    {
        public bool Status { get; set; }
        public virtual ICollection<Brand> Data { get; set; }
    }
}


using System.Collections.Generic;

namespace PhoneShop.DTO.API.PhoneSpecifications
{
    public class Specification
    {
        public string Title { get; set; }
        public virtual ICollection<KeyVal> Specs { get; set; }
    }
}
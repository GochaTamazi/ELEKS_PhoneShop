using System.Collections.Generic;

namespace PhoneShop.DTO.API.PhoneSpecifications
{
    public class KeyVal
    {
        public string Key { get; set; }
        public virtual ICollection<string> Val { get; set; }
    }
}
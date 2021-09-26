using System.Collections.Generic;

namespace PhoneShop.DTO.API.Latest
{
    public class LatestData
    {
        public string Title { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
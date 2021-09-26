using System.Collections.Generic;

namespace PhoneShop.DTO.API.TopByInterest
{
    public class TopByInterestData
    {
        public string Title { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
using System.Collections.Generic;

namespace PhoneShop.DTO.API.Search
{
    public class SearchtData
    {
        public string Title { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
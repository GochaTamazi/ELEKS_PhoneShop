using System.Collections.Generic;

namespace Models.DTO.API.Search
{
    public class SearchtData
    {
        public string Title { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
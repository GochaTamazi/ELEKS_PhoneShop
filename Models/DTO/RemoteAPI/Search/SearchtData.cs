using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.Search
{
    public class SearchtData
    {
        public string Title { get; set; }
        public ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
}
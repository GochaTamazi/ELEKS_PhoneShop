using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.Latest
{
    public class LatestData
    {
        public string Title { get; set; }
        public ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
}
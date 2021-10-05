using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.TopByFans
{
    public class TopByFansData
    {
        public string Title { get; set; }
        public ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
}
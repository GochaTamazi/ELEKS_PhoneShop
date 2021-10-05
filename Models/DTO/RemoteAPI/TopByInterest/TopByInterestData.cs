using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.TopByInterest
{
    public class TopByInterestData
    {
        public string Title { get; set; }
        public ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
}
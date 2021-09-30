using System.Collections.Generic;

namespace Models.DTO.API.TopByInterest
{
    public class TopByInterestData
    {
        public string Title { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
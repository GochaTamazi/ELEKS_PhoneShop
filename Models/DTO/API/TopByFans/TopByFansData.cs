using System.Collections.Generic;

namespace Models.DTO.API.TopByFans
{
    public class TopByFansData
    {
        public string Title { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
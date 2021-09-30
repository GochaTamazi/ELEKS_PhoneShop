using System.Collections.Generic;

namespace Models.DTO.API.PhoneSpecifications
{
    public class KeyVal
    {
        public string Key { get; set; }
        public virtual ICollection<string> Val { get; set; }
    }
}
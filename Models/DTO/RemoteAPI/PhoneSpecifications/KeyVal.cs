using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.PhoneSpecifications
{
    public class KeyVal
    {
        public string Key { get; set; }
        public virtual ICollection<string> Val { get; set; }
    }
}
using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.PhoneSpecifications
{
    public class KeyVal
    {
        public string Key { get; set; }
        public ICollection<string> Val { get; set; } = new List<string>();
    }
}
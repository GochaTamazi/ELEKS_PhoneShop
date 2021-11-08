using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications
{
    public class KeyValDto
    {
        public string Key { get; set; } = string.Empty;
        public ICollection<string> Val { get; set; } = new List<string>();
    }
}
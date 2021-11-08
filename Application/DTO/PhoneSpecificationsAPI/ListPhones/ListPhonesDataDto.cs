using System.Collections.Generic;

namespace Application.DTO.PhoneSpecificationsAPI.ListPhones
{
    public class ListPhonesDataDto
    {
        public string Title { get; set; } = string.Empty;
        public int Current_page { get; set; } = 0;
        public int Last_page { get; set; } = 0;
        public ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
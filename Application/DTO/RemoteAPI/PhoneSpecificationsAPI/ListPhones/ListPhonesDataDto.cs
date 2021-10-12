using System.Collections.Generic;

namespace Application.DTO.RemoteAPI.PhoneSpecificationsAPI.ListPhones
{
    public class ListPhonesDataDto
    {
        public string Title { get; set; }
        public int Current_page { get; set; }
        public int Last_page { get; set; } = 0;
        public ICollection<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
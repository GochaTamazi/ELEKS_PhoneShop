using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.ListPhones
{
    public class ListPhonesData
    {
        public string Title { get; set; }
        public int Current_page { get; set; }
        public int Last_page { get; set; } = 0;
        public ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
}
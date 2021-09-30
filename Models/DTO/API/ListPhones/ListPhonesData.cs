using System.Collections.Generic;

namespace Models.DTO.API.ListPhones
{
    public class ListPhonesData
    {
        public string Title { get; set; }
        public int Current_page { get; set; }
        public int Last_page { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
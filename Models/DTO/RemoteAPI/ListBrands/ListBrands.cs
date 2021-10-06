using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.ListBrands
{
    public class ListBrands
    {
        public bool Status { get; set; } = false;
        public ICollection<Brand> Data { get; set; } = new List<Brand>();
    }
}
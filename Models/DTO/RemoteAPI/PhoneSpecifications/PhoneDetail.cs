using System.Collections.Generic;

namespace Models.DTO.RemoteAPI.PhoneSpecifications
{
    public class PhoneDetail
    {
    public string Brand { get; set; }
    public string Phone_name { get; set; }
    public string Thumbnail { get; set; }
    public virtual ICollection<string> Phone_images { get; set; }
    public string Release_date { get; set; }
    public string Dimension { get; set; }
    public string Os { get; set; }
    public string Storage { get; set; }
    public virtual ICollection<Specification> Specifications { get; set; }
    
    }
}
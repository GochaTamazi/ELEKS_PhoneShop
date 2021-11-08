using Database.Interfaces;

#nullable disable

namespace Database.Models
{
    public partial class PriceSubscriber : IEntity
    {
        public int Id { get; set; }
        public string BrandSlug { get; set; }
        public string PhoneSlug { get; set; }
        public string Email { get; set; }
    }
}
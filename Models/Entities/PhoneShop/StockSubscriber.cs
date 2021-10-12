using Models.Interfaces;

namespace Models.Entities.PhoneShop
{
    public partial class StockSubscriber : IEntity
    {
        public int Id { get; set; }
        public int? PhoneId { get; set; }
        public string Email { get; set; }
    }
}
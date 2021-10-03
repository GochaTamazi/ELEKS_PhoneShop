namespace Models.Entities.PhoneShop
{
    public partial class StockSubscriber
    {
        public int Id { get; set; }
        public int? PhoneId { get; set; }
        public string Email { get; set; }
    }
}

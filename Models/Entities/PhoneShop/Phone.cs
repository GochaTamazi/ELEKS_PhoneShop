namespace Models.Entities.PhoneShop
{
    public partial class Phone
    {
        public int Id { get; set; }
        public int? PhoneId { get; set; }
        public int? Price { get; set; }
        public int? Stock { get; set; }
        public bool? Hided { get; set; }
    }
}

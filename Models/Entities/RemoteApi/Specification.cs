namespace Models.Entities.RemoteApi
{
    public partial class Specification
    {
        public int Id { get; set; }
        public int? BrandId { get; set; }
        public int? PhoneId { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public string Dimension { get; set; }
        public string Os { get; set; }
        public string Storage { get; set; }
        public string Thumbnail { get; set; }
        public string Images { get; set; }
        public string Specifications { get; set; }
    }
}

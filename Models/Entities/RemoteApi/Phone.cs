using Models.Interfaces;

namespace Models.Entities.RemoteApi
{
    public partial class Phone : IEntity
    {
        public int Id { get; set; }
        public int? BrandId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
    }
}
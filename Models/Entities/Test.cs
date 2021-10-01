using Models.Interfaces;

#nullable disable

namespace Models.Entities
{
    public partial class Test: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

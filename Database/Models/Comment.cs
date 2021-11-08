using System;
using Database.Interfaces;

#nullable disable

namespace Database.Models
{
    public partial class Comment : IEntity
    {
        public int Id { get; set; }
        public string PhoneSlug { get; set; }
        public int? UserId { get; set; }
        public string Comments { get; set; }
        public int? Rating { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual User User { get; set; }
    }
}
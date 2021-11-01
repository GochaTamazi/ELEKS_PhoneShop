using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class Comment
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

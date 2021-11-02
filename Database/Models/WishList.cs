using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class WishList
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? PhoneId { get; set; }

        public virtual Phone Phone { get; set; }
        public virtual User User { get; set; }
    }
}

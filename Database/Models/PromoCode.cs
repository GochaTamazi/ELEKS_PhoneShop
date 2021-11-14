using System;
using System.Collections.Generic;
using Database.Interfaces;

#nullable disable

namespace Database.Models
{
    public partial class PromoCode : IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int? Amount { get; set; }
        public int? Discount { get; set; }
        public int? PhoneId { get; set; }

        public virtual Phone Phone { get; set; }
    }
}
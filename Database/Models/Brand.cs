using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace Database.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}

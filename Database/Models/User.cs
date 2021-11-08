using System.Collections.Generic;
using Database.Interfaces;

#nullable disable

namespace Database.Models
{
    public partial class User : IEntity
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            WishLists = new HashSet<WishList>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
using System.Collections.Generic;
using Database.Models;

namespace Application.DTO.Frontend
{
    public class CommentsPage
    {
        public string PhoneSlug { set; get; } = string.Empty;
        public int TotalComments { set; get; } = 0;
        public int TotalPages { set; get; } = 0;
        public int PageSize { set; get; } = 0;
        public int Page { set; get; } = 0;
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
using System;

namespace Application.DTO.Frontend
{
    public class CommentForm
    {
        public string PhoneSlug { get; set; } = string.Empty;
        public string UserMail { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public int? Rating { get; set; } = 0;
        public DateTime? CreateTime { get; set; } = DateTime.Now;
    }
}
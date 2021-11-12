namespace Application.DTO.PhoneSpecificationsAPI.TopByFans
{
    public class PhoneDto
    {
        public string Phone_name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int Favorites { get; set; } = 0;
        public string Detail { get; set; } = string.Empty;
    }
}
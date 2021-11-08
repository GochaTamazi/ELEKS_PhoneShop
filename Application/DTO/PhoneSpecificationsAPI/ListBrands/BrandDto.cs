namespace Application.DTO.PhoneSpecificationsAPI.ListBrands
{
    public class BrandDto
    {
        public int Brand_id { get; set; } = 0;
        public string Brand_name { get; set; } = string.Empty;
        public string Brand_slug { get; set; } = string.Empty;
        public int Device_count { get; set; } = 0;
        public string Detail { get; set; } = string.Empty;
    }
}
namespace Application.DTO.Frontend.Forms
{
    public class PhonesFilterForm
    {
        public string BrandName { get; set; } = string.Empty;
        public string PhoneName { get; set; } = string.Empty;
        public uint PriceMin { get; set; } = 0;
        public uint PriceMax { get; set; } = 10_000;
        public bool InStock { get; set; } = true;
        public string OrderBy { get; set; } = "PhoneName";
    }
}
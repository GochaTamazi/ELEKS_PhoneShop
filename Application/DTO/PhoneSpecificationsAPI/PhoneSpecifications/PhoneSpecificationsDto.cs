namespace Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications
{
    public class PhoneSpecificationsDto
    {
        public bool Status { get; set; } = false;
        public PhoneDetailDto Data { get; set; } = new PhoneDetailDto();
    }
}
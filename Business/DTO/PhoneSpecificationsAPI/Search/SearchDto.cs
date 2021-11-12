namespace Application.DTO.PhoneSpecificationsAPI.Search
{
    public class SearchDto
    {
        public bool Status { get; set; } = false;
        public SearchtDataDto Data { get; set; } = new SearchtDataDto();
    }
}
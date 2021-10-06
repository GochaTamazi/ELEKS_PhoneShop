namespace Models.DTO.RemoteAPI.ListPhones
{
    public class ListPhones
    {
        public bool Status { get; set; } = false;
        public ListPhonesData Data { get; set; }
    }
}
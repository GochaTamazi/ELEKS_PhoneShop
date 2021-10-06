namespace Models.DTO.RemoteAPI.Latest
{
    public class Latest
    {
        public bool Status { get; set; } = false;
        public LatestData Data { get; set; }
    }
}
using System.Net;

namespace Application.DTO.PhoneSpecificationsAPI
{
    public class ApiResponseDto
    {
        public string Message { get; set; } = "";
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.GatewayTimeout;
        public object Data { get; set; }
    }
}
using System.Net.Http;
using System.Threading.Tasks;

namespace PhoneShop.Tools
{
    public class HttpRequest : IHttpRequest
    {
        private readonly HttpClient _client;

        public HttpRequest()
        {
            _client = new HttpClient();
        }

        public async Task<string> GetAsync(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
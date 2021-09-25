using System.Threading.Tasks;
using PhoneShop.Tools;

namespace PhoneShop.RemoteAPI
{
    public class PhoneSpecification : IPhoneSpecification
    {
        private IHttpRequest _httpRequest;

        private string _baseUrl;

        public PhoneSpecification()
        {
            _baseUrl = "http://api-mobilespecs.azharimm.site";
            _httpRequest = new HttpRequest();
        }

        public Task<string> ListBrandsAsync()
        {
            const string endpoint = "/v2/brands";
            var url = $"{_baseUrl}{endpoint}";
            return _httpRequest.GetAsync(url);
        }

        public Task<string> ListPhonesAsync(string brandSlug, int page = 1)
        {
            var endpoint = $"/v2/brands/{brandSlug}?page={page}";
            var url = $"{_baseUrl}{endpoint}";
            return _httpRequest.GetAsync(url);
        }

        public Task<string> PhoneSpecificationsAsync(string phoneSlug)
        {
            var endpoint = $"/v2/{phoneSlug}";
            var url = $"{_baseUrl}{endpoint}";
            return _httpRequest.GetAsync(url);
        }

        public Task<string> SearchAsync(string query)
        {
            var endpoint = $"/v2/search?query={query}";
            var url = $"{_baseUrl}{endpoint}";
            return _httpRequest.GetAsync(url);
        }

        public Task<string> LatestAsync()
        {
            const string endpoint = "/v2/latest";
            var url = $"{_baseUrl}{endpoint}";
            return _httpRequest.GetAsync(url);
        }

        public Task<string> TopByInterestAsync()
        {
            const string endpoint = "/v2/top-by-interest";
            var url = $"{_baseUrl}{endpoint}";
            return _httpRequest.GetAsync(url);
        }

        public Task<string> TopByFansAsync()
        {
            const string endpoint = "/v2/top-by-fans";
            var url = $"{_baseUrl}{endpoint}";
            return _httpRequest.GetAsync(url);
        }
    }
}
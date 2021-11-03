using Application.DTO.Options;
using Application.DTO.PhoneSpecificationsAPI.Latest;
using Application.DTO.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.PhoneSpecificationsAPI.ListPhones;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using Application.DTO.PhoneSpecificationsAPI.Search;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;
using Application.DTO.PhoneSpecificationsAPI.TopByInterest;
using Application.Interfaces;
using Flurl.Http;
using Flurl;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Services
{
    public class PhoneSpecificationsApi : IPhoneSpecificationsApi
    {
        private readonly PhoneSpecificationsApiOptions _options;
        private readonly string _baseUrl;

        public PhoneSpecificationsApi(IOptions<PhoneSpecificationsApiOptions> options)
        {
            _options = options.Value;
            _baseUrl = _options.BaseUrl;
        }

        public async Task<LatestDto> LatestAsync(CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "latest").GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<LatestDto>();
            }

            return new LatestDto();
        }

        public async Task<ListBrandsDto> ListBrandsAsync(CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "brands").GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<ListBrandsDto>();
            }

            return new ListBrandsDto();
        }

        public async Task<ListPhonesDto> ListPhonesAsync(string brandSlug, int page,
            CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "brands", brandSlug)
                .SetQueryParams(new {page = page})
                .GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<ListPhonesDto>();
            }

            return new ListPhonesDto();
        }

        public async Task<ListPhonesDto> ListPhonesAsync2(string brandSlug, int page,
            CancellationToken token)
        {
            using var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync($"{_baseUrl}/v2/brands/{brandSlug}?page={page}", token);

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadFromJsonAsync<ListPhonesDto>(cancellationToken: token);
            }

            return new ListPhonesDto();
        }

        public async Task<PhoneSpecificationsDto> PhoneSpecificationsAsync(string phoneSlug,
            CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", phoneSlug).GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<PhoneSpecificationsDto>();
            }

            return new PhoneSpecificationsDto();
        }

        public async Task<PhoneSpecificationsDto> PhoneSpecificationsAsync2(string phoneSlug,
            CancellationToken token)
        {
            using var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync($"{_baseUrl}/v2/{phoneSlug}", token);

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadFromJsonAsync<PhoneSpecificationsDto>(cancellationToken: token);
            }

            return new PhoneSpecificationsDto();
        }

        public async Task<SearchDto> SearchAsync(string query,
            CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "search")
                .SetQueryParams(new {query = query})
                .GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<SearchDto>();
            }

            return new SearchDto();
        }

        public async Task<TopByFansDto> TopByFansAsync(CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "top-by-fans").GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<TopByFansDto>();
            }

            return new TopByFansDto();
        }

        public async Task<TopByInterestDto> TopByInterestAsync(CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "top-by-interest").GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<TopByInterestDto>();
            }

            return new TopByInterestDto();
        }
    }
}
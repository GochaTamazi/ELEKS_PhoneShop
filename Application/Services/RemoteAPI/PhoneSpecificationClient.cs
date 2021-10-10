using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Flurl;
using Flurl.Http;
using Models.DTO.RemoteAPI.Latest;
using Models.DTO.RemoteAPI.ListBrands;
using Models.DTO.RemoteAPI.ListPhones;
using Models.DTO.RemoteAPI.PhoneSpecifications;
using Models.DTO.RemoteAPI.Search;
using Models.DTO.RemoteAPI.TopByFans;
using Models.DTO.RemoteAPI.TopByInterest;

namespace Application.Services.RemoteAPI
{
    public class PhoneSpecificationClient : IPhoneSpecificationClient
    {
        private readonly string _baseUrl;

        public PhoneSpecificationClient()
        {
            _baseUrl = "http://api-mobilespecs.azharimm.site";
        }

        public async Task<ListBrands> ListBrandsAsync(CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "brands").GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<ListBrands>();
            }

            return new ListBrands();
        }

        /// <summary>
        /// Flurl request
        /// </summary>
        /// <param name="brandSlug"></param>
        /// <param name="page"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ListPhones> ListPhonesAsync(string brandSlug, int page, CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "brands", brandSlug)
                .SetQueryParams(new {page = page})
                .GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<ListPhones>();
            }

            return new ListPhones();
        }

        /// <summary>
        /// System.Net.Http.HttpClient request
        /// </summary>
        /// <param name="brandSlug"></param>
        /// <param name="page"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ListPhones> ListPhonesAsync2(string brandSlug, int page, CancellationToken token)
        {
            using var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync($"{_baseUrl}/v2/brands/{brandSlug}?page={page}", token);

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadFromJsonAsync<ListPhones>(cancellationToken: token);
            }

            return new ListPhones();
        }

        public async Task<PhoneSpecifications> PhoneSpecificationsAsync(string phoneSlug, CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", phoneSlug).GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<PhoneSpecifications>();
            }

            return new PhoneSpecifications();
        }

        public async Task<PhoneSpecifications> PhoneSpecificationsAsync2(string phoneSlug, CancellationToken token)
        {
            using var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync($"{_baseUrl}/v2/{phoneSlug}", token);

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadFromJsonAsync<PhoneSpecifications>(cancellationToken: token);
            }

            return new PhoneSpecifications();
        }

        public async Task<Search> SearchAsync(string query, CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "search")
                .SetQueryParams(new {query = query})
                .GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<Search>();
            }

            return new Search();
        }

        public async Task<Latest> LatestAsync(CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "latest").GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<Latest>();
            }

            return new Latest();
        }

        public async Task<TopByInterest> TopByInterestAsync(CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "top-by-interest").GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<TopByInterest>();
            }

            return new TopByInterest();
        }

        public async Task<TopByFans> TopByFansAsync(CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", "top-by-fans").GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<TopByFans>();
            }

            return new TopByFans();
        }
    }
}
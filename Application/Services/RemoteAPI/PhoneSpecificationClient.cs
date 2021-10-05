using System;
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

        public async Task<PhoneSpecifications> PhoneSpecificationsAsync(string phoneSlug, CancellationToken token)
        {
            var response = _baseUrl.AppendPathSegments("v2", phoneSlug).GetAsync(token);

            if (response.Result.StatusCode == 200)
            {
                return await response.ReceiveJson<PhoneSpecifications>();
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
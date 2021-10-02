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

        public Task<ListBrands> ListBrandsAsync(CancellationToken token)
        {
            return _baseUrl.AppendPathSegments("v2", "brands")
                .GetAsync(token)
                .ReceiveJson<ListBrands>();
        }

        public Task<ListPhones> ListPhonesAsync(string brandSlug, int page, CancellationToken token)
        {
            return _baseUrl.AppendPathSegments("v2", "brands", brandSlug)
                .SetQueryParams(new {page = page})
                .GetAsync(token)
                .ReceiveJson<ListPhones>();
        }

        public Task<PhoneSpecifications> PhoneSpecificationsAsync(string phoneSlug, CancellationToken token)
        {
            return _baseUrl.AppendPathSegments("v2", phoneSlug)
                .GetAsync(token)
                .ReceiveJson<PhoneSpecifications>();
        }

        public Task<Search> SearchAsync(string query, CancellationToken token)
        {
            return _baseUrl.AppendPathSegments("v2", "search")
                .SetQueryParams(new {query = query})
                .GetAsync(token)
                .ReceiveJson<Search>();
        }

        public Task<Latest> LatestAsync(CancellationToken token)
        {
            return _baseUrl.AppendPathSegments("v2", "latest")
                .GetAsync(token)
                .ReceiveJson<Latest>();
        }

        public Task<TopByInterest> TopByInterestAsync(CancellationToken token)
        {
            return _baseUrl.AppendPathSegments("v2", "top-by-interest")
                .GetAsync(token)
                .ReceiveJson<TopByInterest>();
        }

        public Task<TopByFans> TopByFansAsync(CancellationToken token)
        {
            return _baseUrl.AppendPathSegments("v2", "top-by-fans")
                .GetAsync(token)
                .ReceiveJson<TopByFans>();
        }
    }
}
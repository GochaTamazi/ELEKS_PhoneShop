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

        public Task<ListBrands> ListBrandsAsync(CancellationToken ct)
        {
            return _baseUrl.AppendPathSegments("v2", "brands")
                .GetAsync(ct)
                .ReceiveJson<ListBrands>();
        }

        public Task<ListPhones> ListPhonesAsync(CancellationToken ct, string brandSlug, int page = 1)
        {
            return _baseUrl.AppendPathSegments("v2", "brands", brandSlug)
                .SetQueryParams(new {page = page})
                .GetAsync(ct)
                .ReceiveJson<ListPhones>();
        }

        public Task<PhoneSpecifications> PhoneSpecificationsAsync(CancellationToken ct, string phoneSlug)
        {
            return _baseUrl.AppendPathSegments("v2", phoneSlug)
                .GetAsync(ct)
                .ReceiveJson<PhoneSpecifications>();
        }

        public Task<Search> SearchAsync(CancellationToken ct, string query)
        {
            return _baseUrl.AppendPathSegments("v2", "search")
                .SetQueryParams(new {query = query})
                .GetAsync(ct)
                .ReceiveJson<Search>();
        }

        public Task<Latest> LatestAsync(CancellationToken ct)
        {
            return _baseUrl.AppendPathSegments("v2", "latest")
                .GetAsync(ct)
                .ReceiveJson<Latest>();
        }

        public Task<TopByInterest> TopByInterestAsync(CancellationToken ct)
        {
            return _baseUrl.AppendPathSegments("v2", "top-by-interest")
                .GetAsync(ct)
                .ReceiveJson<TopByInterest>();
        }

        public Task<TopByFans> TopByFansAsync(CancellationToken ct)
        {
            return _baseUrl.AppendPathSegments("v2", "top-by-fans")
                .GetAsync(ct)
                .ReceiveJson<TopByFans>();
        }
    }
}
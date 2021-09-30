using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Flurl;
using Flurl.Http;
using Models.DTO.API.Latest;
using Models.DTO.API.ListBrands;
using Models.DTO.API.ListPhones;
using Models.DTO.API.PhoneSpecifications;
using Models.DTO.API.Search;
using Models.DTO.API.TopByFans;
using Models.DTO.API.TopByInterest;

namespace Core.Services
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
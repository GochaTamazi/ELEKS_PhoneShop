using System.Threading.Tasks;
using System.Threading;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using PhoneShop.Configs;
using PhoneShop.DTO.API.Latest;
using PhoneShop.DTO.API.ListBrands;
using PhoneShop.DTO.API.ListPhones;
using PhoneShop.DTO.API.PhoneSpecifications;
using PhoneShop.DTO.API.Search;
using PhoneShop.DTO.API.TopByFans;
using PhoneShop.DTO.API.TopByInterest;

namespace PhoneShop.RemoteAPI
{
    public class PhoneSpecificationClient : IPhoneSpecificationClient
    {
        private readonly string _baseUrl;

        public PhoneSpecificationClient(IOptions<PhoneSpecificationOptions> config)
        {
            _baseUrl = config.Value.Address;
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
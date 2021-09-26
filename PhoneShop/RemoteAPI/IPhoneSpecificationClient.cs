using System.Threading;
using System.Threading.Tasks;
using PhoneShop.DTO.API.Latest;
using PhoneShop.DTO.API.ListBrands;
using PhoneShop.DTO.API.ListPhones;
using PhoneShop.DTO.API.PhoneSpecifications;
using PhoneShop.DTO.API.Search;
using PhoneShop.DTO.API.TopByFans;
using PhoneShop.DTO.API.TopByInterest;

namespace PhoneShop.RemoteAPI
{
    /// <summary>
    /// Rest Api for Phone specifications
    /// https://github.com/azharimm/phone-specs-api
    /// </summary>
    public interface IPhoneSpecificationClient
    {
        Task<ListBrands> ListBrandsAsync(CancellationToken ct);
        Task<ListPhones> ListPhonesAsync(CancellationToken ct, string brandSlug, int page = 1);
        Task<PhoneSpecifications> PhoneSpecificationsAsync(CancellationToken ct, string phoneSlug);
        Task<Search> SearchAsync(CancellationToken ct, string query);
        Task<Latest> LatestAsync(CancellationToken ct);
        Task<TopByInterest> TopByInterestAsync(CancellationToken ct);
        Task<TopByFans> TopByFansAsync(CancellationToken ct);
    }
}
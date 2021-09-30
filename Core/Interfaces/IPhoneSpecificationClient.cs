using System.Threading;
using System.Threading.Tasks;
using Models.DTO.API.Latest;
using Models.DTO.API.ListBrands;
using Models.DTO.API.ListPhones;
using Models.DTO.API.PhoneSpecifications;
using Models.DTO.API.Search;
using Models.DTO.API.TopByFans;
using Models.DTO.API.TopByInterest;

namespace Core.Interfaces
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
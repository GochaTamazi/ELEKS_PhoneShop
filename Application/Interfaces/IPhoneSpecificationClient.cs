using System.Threading;
using System.Threading.Tasks;
using Models.DTO.RemoteAPI.Latest;
using Models.DTO.RemoteAPI.ListBrands;
using Models.DTO.RemoteAPI.ListPhones;
using Models.DTO.RemoteAPI.PhoneSpecifications;
using Models.DTO.RemoteAPI.Search;
using Models.DTO.RemoteAPI.TopByFans;
using Models.DTO.RemoteAPI.TopByInterest;

namespace Application.Interfaces
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
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
        Task<ListBrands> ListBrandsAsync(CancellationToken token);
        Task<ListPhones> ListPhonesAsync(string brandSlug, int page, CancellationToken token);
        Task<ListPhones> ListPhonesAsync2(string brandSlug, int page, CancellationToken token);
        Task<PhoneSpecifications> PhoneSpecificationsAsync(string phoneSlug, CancellationToken token);
        Task<PhoneSpecifications> PhoneSpecificationsAsync2(string phoneSlug, CancellationToken token);
        Task<Search> SearchAsync(string query, CancellationToken token);
        Task<Latest> LatestAsync(CancellationToken token);
        Task<TopByInterest> TopByInterestAsync(CancellationToken token);
        Task<TopByFans> TopByFansAsync(CancellationToken token);
    }
}
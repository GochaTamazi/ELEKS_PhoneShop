using System.Threading;
using System.Threading.Tasks;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.Latest;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.ListPhones;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.PhoneSpecifications;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.Search;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.TopByFans;
using Application.DTO.RemoteAPI.PhoneSpecificationsAPI.TopByInterest;

namespace Application.Interfaces.RemoteAPI
{
    /// <summary>
    /// Phone Specifications API
    /// https://github.com/azharimm/phone-specs-api
    /// </summary>
    public interface IPhoneSpecificationsApi
    {
        Task<ListBrandsDto> ListBrandsAsync(CancellationToken token);
        Task<ListPhonesDto> ListPhonesAsync(string brandSlug, int page, CancellationToken token);
        Task<ListPhonesDto> ListPhonesAsync2(string brandSlug, int page, CancellationToken token);
        Task<PhoneSpecificationsDto> PhoneSpecificationsAsync(string phoneSlug, CancellationToken token);
        Task<PhoneSpecificationsDto> PhoneSpecificationsAsync2(string phoneSlug, CancellationToken token);
        Task<SearchDto> SearchAsync(string query, CancellationToken token);
        Task<LatestDto> LatestAsync(CancellationToken token);
        Task<TopByInterestDto> TopByInterestAsync(CancellationToken token);
        Task<TopByFansDto> TopByFansAsync(CancellationToken token);
    }
}
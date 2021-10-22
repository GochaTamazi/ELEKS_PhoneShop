using Application.DTO.PhoneSpecificationsAPI.Latest;
using Application.DTO.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.PhoneSpecificationsAPI.ListPhones;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using Application.DTO.PhoneSpecificationsAPI.Search;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;
using Application.DTO.PhoneSpecificationsAPI.TopByInterest;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    /// <summary>
    /// Phone Specifications API
    /// https://github.com/azharimm/phone-specs-api
    /// </summary>
    public interface IPhoneSpecificationsApi
    {
        Task<LatestDto> LatestAsync(CancellationToken token);
        Task<ListBrandsDto> ListBrandsAsync(CancellationToken token);
        Task<ListPhonesDto> ListPhonesAsync(string brandSlug, int page, CancellationToken token);
        Task<ListPhonesDto> ListPhonesAsync2(string brandSlug, int page, CancellationToken token);
        Task<PhoneSpecificationsDto> PhoneSpecificationsAsync(string phoneSlug, CancellationToken token);
        Task<PhoneSpecificationsDto> PhoneSpecificationsAsync2(string phoneSlug, CancellationToken token);
        Task<SearchDto> SearchAsync(string query, CancellationToken token);
        Task<TopByFansDto> TopByFansAsync(CancellationToken token);
        Task<TopByInterestDto> TopByInterestAsync(CancellationToken token);
    }
}
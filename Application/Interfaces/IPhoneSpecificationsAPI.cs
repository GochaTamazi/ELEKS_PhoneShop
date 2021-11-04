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
        Task<LatestDto> GetLatestAsync(CancellationToken token);

        Task<ListBrandsDto> GetListBrandsOrThrowAsync(CancellationToken token);

        Task<ListPhonesDto> GetListPhonesAsync(string brandSlug, int page,
            CancellationToken token);

        Task<ListPhonesDto> GetListPhonesAsync2(string brandSlug, int page,
            CancellationToken token);

        Task<PhoneSpecificationsDto> GetPhoneSpecificationsOrThrowAsync(string phoneSlug,
            CancellationToken token);

        Task<PhoneSpecificationsDto> GetPhoneSpecificationsAsync2(string phoneSlug,
            CancellationToken token);

        Task<SearchDto> SearchAsync(string query,
            CancellationToken token);

        Task<TopByFansDto> GetTopByFansAsync(CancellationToken token);

        Task<TopByInterestDto> GetTopByInterestAsync(CancellationToken token);
    }
}
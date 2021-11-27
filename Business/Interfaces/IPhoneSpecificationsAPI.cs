using Application.DTO.PhoneSpecificationsAPI;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface IPhoneSpecificationsApi
    {
        Task<ApiResponseDto> GetListBrandsAsync(CancellationToken token);

        Task<ApiResponseDto> GetListPhonesAsync(string brandSlug, int page, CancellationToken token);

        Task<ApiResponseDto> GetPhoneSpecificationsAsync(string phoneSlug, CancellationToken token);

        Task<ApiResponseDto> GetLatestAsync(CancellationToken token);

        Task<ApiResponseDto> GetTopByFansAsync(CancellationToken token);

        Task<ApiResponseDto> GetTopByInterestAsync(CancellationToken token);

        Task<ApiResponseDto> SearchAsync(string query, CancellationToken token);
    }
}
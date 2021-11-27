using System.Collections.Generic;
using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;
using Application.DTO.PhoneSpecificationsAPI;
using Database.Models;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task<ApiResponseDto> GetOneAsync(string phoneSlug, CancellationToken token);

        Task<List<Phone>> GetAllAsync(PhonesFilterForm filterForm, CancellationToken token);

        Task<PhonesPageFront> GetAllPagedAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token);

        Task<Phone> AddOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token);
        
        Task AddOrUpdateAsync(List<Phone> phones, CancellationToken token);
    }
}
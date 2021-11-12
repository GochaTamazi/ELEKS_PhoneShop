using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface ICustomerPhones
    {
        Task<PhoneDto> GetOneAsync(string phoneSlug, CancellationToken token);

        Task<PhonesPageFront> GetAllAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token);
    }
}
using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token);

        Task<PhoneSpecFront> GetPhoneAsync(string phoneSlug, CancellationToken token);

        Task<PhonesPageFront> GetPhonesAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token);
    }
}
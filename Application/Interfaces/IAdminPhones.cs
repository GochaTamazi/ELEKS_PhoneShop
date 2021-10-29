using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;
using Application.DTO.Frontend.Forms;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task<PhoneSpecFront> GetPhoneAsync(string phoneSlug, CancellationToken token);
        Task<PhonesPageFront> GetPhonesAsync(PhonesFilterForm filterForm, int page, int pageSize, CancellationToken token);
        Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token);
    }
}
using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;
using Database.Models;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task<Phone> PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token);

        Task<PhoneSpecFront> GetPhoneAsync(string phoneSlug, CancellationToken token);

        Task<PhonesPageFront> GetPhonesAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token);
    }
}
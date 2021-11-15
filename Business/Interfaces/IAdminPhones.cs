using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;
using Database.Models;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task<PhoneSpecFront> GetOneAsync(string phoneSlug, CancellationToken token);

        Task<PhonesPageFront> GetAllAsync(PhonesFilterForm filterForm, int page, int pageSize, CancellationToken token);

        Task<Phone> AddOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token);
    }
}
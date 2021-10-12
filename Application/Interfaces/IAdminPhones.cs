using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task<PhoneSpecFront> GetPhone(string phoneSlug, CancellationToken token);
        Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token);
        Task BrandInsertIfNotExistAsync(string brandSlug, CancellationToken token);
    }
}
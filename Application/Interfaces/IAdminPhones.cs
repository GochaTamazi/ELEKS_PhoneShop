using Application.DTO.Frontend;
using Database.Models;
using System.Threading.Tasks;
using System.Threading;
using Application.DTO.Frontend.Forms;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task BrandInsertIfNotExistAsync(string brandSlug, CancellationToken token);
        Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token);
        Task PriceSubscribersNotificationAsync(Phone phone, CancellationToken token);
        Task StockSubscribersNotificationAsync(Phone phone, CancellationToken token);

        Task<PhonesPageFront> GetPhonesInStoreAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token);

        Task<PhoneSpecFront> GetPhone(string phoneSlug, CancellationToken token);
    }
}
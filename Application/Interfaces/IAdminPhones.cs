using Application.DTO.Frontend;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task BrandInsertIfNotExistAsync(string brandSlug, CancellationToken token);
        Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token);
        Task PriceSubscribersNotificationAsync(Phone phone, CancellationToken token);
        Task StockSubscribersNotificationAsync(Phone phone, CancellationToken token);
        Task<List<Phone>> GetPhonesInStoreAsync(CancellationToken token);
        Task<PhoneSpecFront> GetPhone(string phoneSlug, CancellationToken token);
    }
}
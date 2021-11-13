using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;

namespace Application.Interfaces
{
    public interface ICustomerCart
    {
        Task<List<PhoneDto>> GetAllAsync();
        
        Task InsertAsync(string phoneSlug, string userMail, CancellationToken token);

        Task DeleteAsync(string phoneSlug, string userMail, CancellationToken token);

        Task BuyAsync(string userMail, CancellationToken token);

        Task UsePromoCodeAsync(string code, string userMail, CancellationToken token);
    }
}
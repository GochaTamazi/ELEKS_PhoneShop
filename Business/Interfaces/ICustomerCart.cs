using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;

namespace Application.Interfaces
{
    public interface ICustomerCart
    {
        Task AddAsync(string phoneSlug, string userMail, CancellationToken token);
        Task RemoveAsync(string phoneSlug, string userMail, CancellationToken token);
        Task BuyAsync(string userMail, CancellationToken token);
        Task UsePromoCodeAsync(string code, string userMail, CancellationToken token);
        Task<List<PhoneDto>> GetAllAsync();
    }
}
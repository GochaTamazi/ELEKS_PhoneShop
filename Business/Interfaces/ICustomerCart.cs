using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace Application.Interfaces
{
    public interface ICustomerCart
    {
        Task<List<Cart>> GetAllAsync(string userMail, CancellationToken token);

        Task InsertOrUpdateAsync(string phoneSlug, string userMail, int amount, CancellationToken token);

        Task DeleteAsync(string phoneSlug, string userMail, CancellationToken token);

        Task BuyAsync(string userMail, CancellationToken token);

        Task UsePromoCodeAsync(string code, string userMail, CancellationToken token);
    }
}
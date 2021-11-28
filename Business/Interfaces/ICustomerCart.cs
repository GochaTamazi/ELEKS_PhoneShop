using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface ICustomerCart
    {
        Task<List<Cart>> GetAllAsync(string userMail, CancellationToken token);

        Task AddOrUpdateAsync(string phoneSlug, string userMail, int amount, CancellationToken token);

        Task RemoveAsync(string phoneSlug, string userMail, CancellationToken token);

        Task<List<Cart>> BuyAsync(string userMail, CancellationToken token);
    }
}
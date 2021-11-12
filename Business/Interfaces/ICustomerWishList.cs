using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace Application.Interfaces
{
    public interface ICustomerWishList
    {
        Task AddAsync(string phoneSlug, string userMail, CancellationToken token);

        Task RemoveAsync(string phoneSlug, string userMail, CancellationToken token);

        Task<List<WishList>> GetAllAsync(string userMail, CancellationToken token);
    }
}
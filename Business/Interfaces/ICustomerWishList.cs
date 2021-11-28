using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface ICustomerWishList
    {
        Task<List<WishList>> GetAllAsync(string userMail, CancellationToken token);

        Task AddIfNotExistAsync(string phoneSlug, string userMail, CancellationToken token);

        Task RemoveAsync(string phoneSlug, string userMail, CancellationToken token);
    }
}
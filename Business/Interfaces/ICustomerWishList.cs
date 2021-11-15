using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace Application.Interfaces
{
    public interface ICustomerWishList
    {
        Task<List<WishList>> GetAllAsync(string userMail, CancellationToken token);

        Task InsertIfNotExistAsync(string phoneSlug, string userMail, CancellationToken token);

        Task DeleteAsync(string phoneSlug, string userMail, CancellationToken token);
    }
}
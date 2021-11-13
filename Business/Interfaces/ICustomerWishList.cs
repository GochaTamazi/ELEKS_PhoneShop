using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace Application.Interfaces
{
    public interface ICustomerWishList
    {
        Task<List<WishList>> GetAllAsync(string userMail, CancellationToken token);

        Task InsertAsync(string phoneSlug, string userMail, CancellationToken token);

        Task DeleteAsync(string phoneSlug, string userMail, CancellationToken token);
    }
}
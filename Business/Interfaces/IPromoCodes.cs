using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend.Forms;
using Database.Models;

namespace Application.Interfaces
{
    public interface IPromoCodes
    {
        Task<List<PromoCode>> GetAllAsync(CancellationToken token);

        Task<PromoCode> GetOneAsync(string key, CancellationToken token);

        Task AddOrUpdateAsync(PromoCodeForm promoCodeForm, CancellationToken token);

        Task RemoveIfExistAsync(string key, CancellationToken token);

        Task<double> Buy(List<Cart> carts, string key, CancellationToken token);
    }
}
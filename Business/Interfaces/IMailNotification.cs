using Database.Models;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface IMailNotification
    {
        Task NotifyPriceSubscribersAsync(Phone phone, CancellationToken token);

        Task NotifyPriceWishListCustomerAsync(Phone phone, CancellationToken token);

        Task NotifyStockSubscribersAsync(Phone phone, CancellationToken token);
    }
}
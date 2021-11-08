using Database.Models;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface IMailNotification
    {
        Task PriceSubscribersNotificationAsync(Phone phone, CancellationToken token);

        Task PriceWishListCustomerNotificationAsync(Phone phone, CancellationToken token);

        Task StockSubscribersNotificationAsync(Phone phone, CancellationToken token);
    }
}
using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface IMailNotification
    {
        Task PriceSubscribersNotificationAsync(List<PriceSubscriber> subscribers, Phone phone, CancellationToken token);
        Task StockSubscribersNotificationAsync(List<StockSubscriber> subscribers, Phone phone, CancellationToken token);
    }
}
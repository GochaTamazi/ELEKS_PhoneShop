using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities;

namespace Application.Interfaces
{
    public interface IMailNotification
    {
        Task PriceSubscribersNotificationAsync(List<PriceSubscriber> subs, Phone phone, CancellationToken token);
        Task StockSubscribersNotificationAsync(List<StockSubscriber> subs, Phone phone, CancellationToken token);
    }
}
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend.Forms;

namespace Application.Interfaces
{
    public interface ISubscribers
    {
        Task SubscribeOnPriceAsync(SubscriberForm subscriberForm, CancellationToken token);

        Task SubscribeOnStockAsync(SubscriberForm subscriberForm, CancellationToken token);
    }
}
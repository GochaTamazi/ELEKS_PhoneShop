using Application.DTO.Frontend.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface ISubscribers
    {
        Task SubscribeOnPriceAsync(SubscriberForm subscriberForm, CancellationToken token);

        Task SubscribeOnStockAsync(SubscriberForm subscriberForm, CancellationToken token);
    }
}
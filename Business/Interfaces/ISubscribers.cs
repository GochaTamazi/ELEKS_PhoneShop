using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend.Forms;

namespace Application.Interfaces
{
    public interface ISubscribers
    {
        Task SubscribeOnPriceAsync(PriceSubscriberForm priceSubscriberForm, CancellationToken token);

        Task SubscribeOnStockAsync(StockSubscriberForm stockSubscriberForm, CancellationToken token);
    }
}
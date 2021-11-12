using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend.Forms;

namespace Application.Interfaces
{
    public interface ISubscribers
    {
        Task PriceChangingAsync(PriceSubscriberForm priceSubscriberForm, CancellationToken token);

        Task StockChangingAsync(StockSubscriberForm stockSubscriberForm, CancellationToken token);
    }
}
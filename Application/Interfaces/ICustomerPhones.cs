using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface ICustomerPhones
    {
        Task SubscribePriceAsync(PriceSubscriberFront priceSubscriberFront, CancellationToken token);
        Task SubscribeStockAsync(StockSubscriberFront stockSubscriberFront, CancellationToken token);
        Task<PhonesPageFront> GetPhonesAsync(PhonesFilter filter, int page, int pageSize, CancellationToken token);
        Task<PhoneDto> GetPhoneAsync(string phoneSlug, CancellationToken token);
    }
}
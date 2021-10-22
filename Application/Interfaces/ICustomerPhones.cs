using Application.DTO.Frontend;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface ICustomerPhones
    {
        Task SubscribePriceAsync(PriceSubscriberFront priceSubscriberFront, CancellationToken token);
        Task SubscribeStockAsync(StockSubscriberFront stockSubscriberFront, CancellationToken token);
        Task<List<Phone>> GetPhonesAsync(PhonesFilter filter, CancellationToken token);
        Task<PhoneDto> GetPhoneAsync(string phoneSlug, CancellationToken token);
    }
}
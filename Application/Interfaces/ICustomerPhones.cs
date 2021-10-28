using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;
using Application.DTO.Frontend.Forms;

namespace Application.Interfaces
{
    public interface ICustomerPhones
    {
        Task SubscribePriceAsync(PriceSubscriberForm priceSubscriberForm, CancellationToken token);
        Task SubscribeStockAsync(StockSubscriberForm stockSubscriberForm, CancellationToken token);
        Task<PhonesPageFront> GetPhonesAsync(PhonesFilterForm filterForm, int page, int pageSize, CancellationToken token);
        Task<PhoneDto> GetPhoneAsync(string phoneSlug, CancellationToken token);
        Task<bool> PostComment(CommentForm commentForm, CancellationToken token);


        //Task<PhoneDto> GetCurrentUserId(CancellationToken token);
    }
}
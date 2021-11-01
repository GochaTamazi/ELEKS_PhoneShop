using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;
using Application.DTO.Frontend.Forms;

namespace Application.Interfaces
{
    public interface ICustomerPhones
    {
        Task<PhoneDto> GetPhoneAsync(string phoneSlug, CancellationToken token);
        Task<CommentsPage> GetPhoneCommentsAsync(string phoneSlug, int page, int pageSize, CancellationToken token);
        Task<PhonesPageFront> GetPhonesAsync(PhonesFilterForm filterForm, int page, int pageSize, CancellationToken token);
        Task SubscribePriceAsync(PriceSubscriberForm priceSubscriberForm, CancellationToken token);
        Task SubscribeStockAsync(StockSubscriberForm stockSubscriberForm, CancellationToken token);
        Task<bool> PostCommentAsync(CommentForm commentForm, CancellationToken token);
    }
}
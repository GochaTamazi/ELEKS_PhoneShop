using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface ICustomerPhones
    {
        Task AddToWishListAsync(string phoneSlug, string userMail,
            CancellationToken token);

        Task RemoveFromWishListAsync(string phoneSlug, string userMail,
            CancellationToken token);

        Task SubscribePriceAsync(PriceSubscriberForm priceSubscriberForm,
            CancellationToken token);

        Task SubscribeStockAsync(StockSubscriberForm stockSubscriberForm,
            CancellationToken token);

        Task<CommentsPage> GetPhoneCommentsAsync(string phoneSlug, int page, int pageSize,
            CancellationToken token);

        Task<List<WishList>> ShowWishListAsync(string userMail,
            CancellationToken token);

        Task<PhoneDto> GetPhoneAsync(string phoneSlug,
            CancellationToken token);

        Task<PhonesPageFront> GetPhonesAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token);

        Task<bool> PostCommentAsync(CommentForm commentForm,
            CancellationToken token);
    }
}
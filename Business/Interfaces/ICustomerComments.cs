using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend;
using Application.DTO.Frontend.Forms;

namespace Application.Interfaces
{
    public interface ICustomerComments
    {
        Task<CommentsPage> GetAllAsync(string phoneSlug, int page, int pageSize, CancellationToken token);

        Task<bool> AddOrUpdateAsync(CommentForm commentForm, CancellationToken token);
    }
}
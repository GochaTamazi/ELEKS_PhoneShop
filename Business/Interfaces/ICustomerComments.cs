using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface ICustomerComments
    {
        Task<CommentsPage> GetAllAsync(string phoneSlug, int page, int pageSize, CancellationToken token);

        Task<bool> AddOrUpdateAsync(CommentForm commentForm, CancellationToken token);
    }
}
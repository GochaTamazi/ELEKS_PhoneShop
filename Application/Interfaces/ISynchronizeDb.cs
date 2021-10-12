using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISynchronizeDb
    {
        Task BrandsAsync(CancellationToken token);
        Task PhonesAsync(CancellationToken token);
        Task SpecificationsAsync(CancellationToken token);
    }
}
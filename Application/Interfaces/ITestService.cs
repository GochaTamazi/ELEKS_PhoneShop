using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITestService
    {
        Task RunTest(CancellationToken token);
    }
}
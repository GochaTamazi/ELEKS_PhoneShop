using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities.RemoteApi;

namespace DataAccess.Interfaces
{
    public interface ISpecificationRep
    {
        Task BulkInsertOrUpdate(List<Specification> entities, CancellationToken token);
    }
}
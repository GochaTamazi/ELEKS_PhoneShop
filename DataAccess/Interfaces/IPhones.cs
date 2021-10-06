using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities.RemoteApi;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// Repository for Models.Entities.RemoteApi.Phone
    /// </summary>
    public interface IPhones
    {
        Task<Phone> GetAsync(int id, CancellationToken token);
        Task<Phone> GetAsync(string slug, CancellationToken token);
        Task<IEnumerable<Phone>> ListAsync(CancellationToken token);
        Task<IEnumerable<Phone>> ListAsync(Expression<Func<Phone, bool>> predicate, CancellationToken token);
        Task InsertAsync(Phone entity, CancellationToken token);
        Task UpdateAsync(Phone entity, CancellationToken token);
        Task DeleteAsync(Phone entity, CancellationToken token);
        Task InsertIfNotExistsAsync(Phone entity, CancellationToken token);
        Task UpdateOrInsertAsync(Phone entity, CancellationToken token);
        Task QueueUpdateOrInsertAsync(Phone entity, CancellationToken token);
    }
}
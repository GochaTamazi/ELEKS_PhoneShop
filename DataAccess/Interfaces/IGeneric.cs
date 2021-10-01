#nullable enable
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Models.Interfaces;

namespace DataAccess.Interfaces
{
    public interface IGeneric<T> where T : IEntity
    {
        Task<T?> GetAsync(int id, CancellationToken token);
        Task<IEnumerable<T>> ListAsync(CancellationToken token);
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken token);
        Task InsertAsync(T entity, CancellationToken token);
        Task UpdateAsync(T entity, CancellationToken token);
        Task DeleteAsync(T entity, CancellationToken token);
    }
}
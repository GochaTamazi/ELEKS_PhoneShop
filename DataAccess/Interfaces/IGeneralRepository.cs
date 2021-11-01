using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IGeneralRepository<T>
    {
        Task<T> GetOneAsync(Expression<Func<T, bool>> condition, CancellationToken token);
        
        Task<List<T>> GetAllAsync(CancellationToken token);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> condition, CancellationToken token);
        Task<List<T>> GetAllAsync<TKey>(Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, CancellationToken token);
        
        Task InsertAsync(T model, CancellationToken token);
        
        Task UpdateAsync(T model, CancellationToken token);
        
        void DetachEntity(T model);

        Task<double?> AverageAsync(Expression<Func<T, bool>> condition, Expression<Func<T, int?>> selector, CancellationToken token);
    }
}
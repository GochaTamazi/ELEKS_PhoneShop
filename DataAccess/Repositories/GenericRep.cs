#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

namespace DataAccess.Repositories
{
    public class GenericRep<T> : IGenericRep<T> where T : class, IEntity
    {
        private readonly MasterContext _masterContext;

        public GenericRep(MasterContext dbContext)
        {
            _masterContext = dbContext;
        }

        public virtual async Task<T?> GetAsync(int id, CancellationToken token)
        {
            return await _masterContext.Set<T>().Where(v =>
                v.Id == id
            ).FirstOrDefaultAsync(token);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken token)
        {
            return await _masterContext.Set<T>().ToListAsync(token);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate,
            CancellationToken token)
        {
            return await _masterContext.Set<T>().Where(predicate).ToListAsync(token);
        }

        public async Task InsertAsync(T entity, CancellationToken token)
        {
            await _masterContext.Set<T>().AddAsync(entity, token);
            await _masterContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            _masterContext.Update(entity);
            await _masterContext.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(T entity, CancellationToken token)
        {
            _masterContext.Set<T>().Remove(entity);
            await _masterContext.SaveChangesAsync(token);
        }
    }
}
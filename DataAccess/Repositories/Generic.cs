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
    public class Generic<T> : IGeneric<T> where T : class, IEntity
    {
        private MasterContext MasterContext { get; set; }

        public Generic(MasterContext dbContext)
        {
            MasterContext = dbContext;
        }

        public virtual async Task<T?> GetAsync(int id, CancellationToken token)
        {
            return await MasterContext.Set<T>().Where(v =>
                v.Id == id
            ).FirstOrDefaultAsync(token);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken token)
        {
            return await MasterContext.Set<T>().ToListAsync(token);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate,
            CancellationToken token)
        {
            return await MasterContext.Set<T>().Where(predicate).ToListAsync(token);
        }

        public async Task InsertAsync(T entity, CancellationToken token)
        {
            await MasterContext.Set<T>().AddAsync(entity, token);
            await MasterContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            //_masterContext.Update(entity).State = EntityState.Modified;
            MasterContext.Entry(entity).State = EntityState.Modified;
            await MasterContext.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(T entity, CancellationToken token)
        {
            MasterContext.Set<T>().Remove(entity);
            await MasterContext.SaveChangesAsync(token);
        }
    }
}
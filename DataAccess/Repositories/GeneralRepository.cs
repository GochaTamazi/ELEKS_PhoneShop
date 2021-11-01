using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {
        private readonly MasterContext _masterContext;

        private readonly DbSet<T> _table = null;

        public GeneralRepository(MasterContext masterContext)
        {
            _masterContext = masterContext;
            _table = _masterContext.Set<T>();
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> condition, CancellationToken token)
        {
            return await _table.Where(condition).FirstOrDefaultAsync(token);
        }

        public async Task<List<T>> GetAllAsync(CancellationToken token)
        {
            return await _table.ToListAsync(token);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> condition, CancellationToken token)
        {
            return await _table.Where(condition).ToListAsync(token);
        }

        public async Task<List<T>> GetAllAsync<TKey>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TKey>> orderBy,
            CancellationToken token
        )
        {
            return await _table.Where(condition).OrderBy(orderBy).ToListAsync(token);
        }

        public async Task InsertAsync(T model, CancellationToken token)
        {
            await _table.AddAsync(model, token);
            await _masterContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(T model, CancellationToken token)
        {
            _table.Update(model);
            await _masterContext.SaveChangesAsync(token);
        }

        public void DetachEntity(T model)
        {
            _masterContext.Entry(model).State = EntityState.Detached;
        }

        public async Task<double?> AverageAsync(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, int?>> selector,
            CancellationToken token
        )
        {
            return await _table.Where(condition).AverageAsync(selector, token);
        }
    }
}
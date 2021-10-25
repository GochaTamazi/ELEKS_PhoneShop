using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class StockSubscribersRepository : IStockSubscribersRepository
    {
        private readonly MasterContext _masterContext;

        public StockSubscribersRepository(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }

        public async Task<StockSubscriber> GetOneAsync(Expression<Func<StockSubscriber, bool>> predicate,
            CancellationToken token)
        {
            return await _masterContext.StockSubscribers.Where(predicate).FirstOrDefaultAsync(token);
        }

        public async Task InsertAsync(StockSubscriber stockSubs, CancellationToken token)
        {
            await _masterContext.StockSubscribers.AddAsync(stockSubs, token);
            await _masterContext.SaveChangesAsync(token);
        }

        public async Task<List<StockSubscriber>> GetAllAsync(Expression<Func<StockSubscriber, bool>> predicate,
            CancellationToken token)
        {
            return await _masterContext.StockSubscribers.Where(predicate).ToListAsync(token);
        }
    }
}
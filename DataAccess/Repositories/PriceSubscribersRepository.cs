using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database;
using Database.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PriceSubscribersRepository : IPriceSubscribersRepository
    {
        private readonly MasterContext _masterContext;

        public PriceSubscribersRepository(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }

        public async Task<PriceSubscriber> GetOneAsync(Expression<Func<PriceSubscriber, bool>> predicate,
            CancellationToken token)
        {
            return await _masterContext.PriceSubscribers.Where(predicate).FirstOrDefaultAsync(token);
        }

        public async Task InsertAsync(PriceSubscriber priceSubs, CancellationToken token)
        {
            await _masterContext.PriceSubscribers.AddAsync(priceSubs, token);
            await _masterContext.SaveChangesAsync(token);
        }

        public async Task<List<PriceSubscriber>> GetAllAsync(Expression<Func<PriceSubscriber, bool>> predicate,
            CancellationToken token)
        {
            return await _masterContext.PriceSubscribers.Where(predicate).ToListAsync(token);
        }
    }
}
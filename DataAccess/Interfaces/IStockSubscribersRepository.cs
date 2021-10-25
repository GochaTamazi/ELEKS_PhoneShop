using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace DataAccess.Interfaces
{
    public interface IStockSubscribersRepository
    {
        Task<StockSubscriber> GetOneAsync(Expression<Func<StockSubscriber, bool>> predicate, CancellationToken token);
        Task InsertAsync(StockSubscriber stockSubs, CancellationToken token);

        Task<List<StockSubscriber>> GetAllAsync(Expression<Func<StockSubscriber, bool>> predicate,
            CancellationToken token);
    }
}
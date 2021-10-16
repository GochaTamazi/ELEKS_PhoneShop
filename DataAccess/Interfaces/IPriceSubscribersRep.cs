using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities;

namespace DataAccess.Interfaces
{
    public interface IPriceSubscribersRep
    {
        Task<PriceSubscriber> GetOneAsync(Expression<Func<PriceSubscriber, bool>> predicate, CancellationToken token);
        Task InsertAsync(PriceSubscriber priceSubs, CancellationToken token);
    }
}
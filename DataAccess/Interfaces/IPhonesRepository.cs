using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities;

namespace DataAccess.Interfaces
{
    public interface IPhonesRepository
    {
        Task<List<Phone>> GetAllAsync(CancellationToken token);

        Task<List<Phone>> GetAllAsync(
            Expression<Func<Phone, bool>> condition,
            CancellationToken token);

        Task<List<Phone>> GetAllAsync<TKey>(
            Expression<Func<Phone, bool>> condition,
            Expression<Func<Phone, TKey>> orderBy,
            CancellationToken token);

        Task<Phone> GetOneAsync(Expression<Func<Phone, bool>> predicate, CancellationToken token);
        Task<Phone> GetPhoneBySlugAsync(string slug, CancellationToken token);
        Task InsertAsync(Phone phone, CancellationToken token);
        Task UpdateAsync(Phone phone, CancellationToken token);
        Task InsertOrUpdateAsync(Phone phone, CancellationToken token);
        Task DetachEntityAsync(Phone phone, CancellationToken token);
    }
}
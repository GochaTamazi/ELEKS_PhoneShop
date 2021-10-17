using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities;

namespace DataAccess.Interfaces
{
    public interface IPhonesRep
    {
        Task<Phone> GetOneAsync(Expression<Func<Phone, bool>> predicate, CancellationToken token);
        Task<List<Phone>> GetAllAsync(Expression<Func<Phone, bool>> predicate, CancellationToken token);
        Task<List<Phone>> GetAllAsync(CancellationToken token);
        Task<Phone> GetPhoneBySlugAsync(string slug, CancellationToken token);
        Task InsertAsync(Phone phone, CancellationToken token);
        Task UpdateAsync(Phone phone, CancellationToken token);
        Task InsertOrUpdateAsync(Phone phone, CancellationToken token);
        void DetachEntity(Phone phone, CancellationToken token);
    }
}
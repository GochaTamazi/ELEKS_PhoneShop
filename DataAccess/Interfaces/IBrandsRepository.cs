using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace DataAccess.Interfaces
{
    public interface IBrandsRepository
    {
        Task InsertAsync(Brand brand, CancellationToken token);
        Task<Brand> GetOneAsync(Expression<Func<Brand, bool>> predicate, CancellationToken token);
    }
}
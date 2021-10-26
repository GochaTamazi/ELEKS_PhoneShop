using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace DataAccess.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetOneAsync(Expression<Func<User, bool>> predicate, CancellationToken token);
        Task InsertAsync(User user, CancellationToken token);
    }
}
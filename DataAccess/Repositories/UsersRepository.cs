using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly MasterContext _masterContext;

        public UsersRepository(MasterContext dbContext)
        {
            _masterContext = dbContext;
        }

        public async Task<User> GetOneAsync(
            Expression<Func<User, bool>> predicate,
            CancellationToken token)
        {
            return await _masterContext.Users.Where(predicate).FirstOrDefaultAsync(token);
        }

        public async Task InsertAsync(User user, CancellationToken token)
        {
            await _masterContext.Users.AddAsync(user, token);
            await _masterContext.SaveChangesAsync(token);
        }
    }
}
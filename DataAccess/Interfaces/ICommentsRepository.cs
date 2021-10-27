using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace DataAccess.Interfaces
{
    public interface ICommentsRepository
    {
        Task<Comment> GetOneAsync(Expression<Func<Comment, bool>> predicate, CancellationToken token);
        Task<List<Comment>> GetAllAsync(Expression<Func<Comment, bool>> predicate, CancellationToken token);
        Task InsertAsync(Comment comment, CancellationToken token);
        Task UpdateAsync(Comment comment, CancellationToken token);
        void DetachEntity(Comment comment);
    }
}
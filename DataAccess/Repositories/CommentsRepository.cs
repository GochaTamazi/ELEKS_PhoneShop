using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly MasterContext _masterContext;

        public CommentsRepository(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }

        public async Task<Comment> GetOneAsync(Expression<Func<Comment, bool>> predicate, CancellationToken token)
        {
            return await _masterContext.Comments.Where(predicate).FirstOrDefaultAsync(token);
        }

        public async Task InsertAsync(Comment comment, CancellationToken token)
        {
            await _masterContext.Comments.AddAsync(comment, token);
            await _masterContext.SaveChangesAsync(token);
        }

        public async Task<List<Comment>> GetAllAsync(Expression<Func<Comment, bool>> predicate, CancellationToken token)
        {
            return await _masterContext.Comments
                .Where(predicate)
                .ToListAsync(token);
        }

        public async Task UpdateAsync(Comment comment, CancellationToken token)
        {
            _masterContext.Update(comment);
            await _masterContext.SaveChangesAsync(token);
        }

        public void DetachEntity(Comment comment)
        {
            _masterContext.Entry(comment).State = EntityState.Detached;
        }
    }
}
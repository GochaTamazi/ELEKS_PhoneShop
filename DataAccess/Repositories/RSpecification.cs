using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities.RemoteApi;
using DataAccess.Interfaces;
using Database;

namespace DataAccess.Repositories
{
    public class RSpecification : ISpecification
    {
        private readonly MasterContext _masterContext;

        public RSpecification(MasterContext dbContext, IGeneric<Phone> genRPhone)
        {
            _masterContext = dbContext;
        }

        public async Task BulkInsertOrUpdate(List<Specification> entities, CancellationToken token)
        {
            await _masterContext.BulkUpdateAsync(entities,
                cancellationToken: token,
                options: options => { options.ColumnPrimaryKeyExpression = specification => specification.PhoneId; }
            );
            await _masterContext.BulkInsertAsync(entities,
                cancellationToken: token,
                options: options =>
                {
                    options.InsertIfNotExists = true;
                    options.ColumnPrimaryKeyExpression = specification => specification.PhoneId;
                }
            );
        }
    }
}
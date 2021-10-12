using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities.RemoteApi;
using DataAccess.Interfaces;
using Database;

namespace DataAccess.Repositories
{
    public class SpecificationRep : ISpecificationRep
    {
        private readonly MasterContext _masterContext;
        private readonly IGenericRep<Specification> _genSpecification;

        public SpecificationRep(MasterContext dbContext, IGenericRep<Specification> genSpecification)
        {
            _masterContext = dbContext;
            _genSpecification = genSpecification;
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
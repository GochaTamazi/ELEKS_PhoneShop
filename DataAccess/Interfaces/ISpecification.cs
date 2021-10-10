using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Models.Entities.RemoteApi;
using DataAccess.Interfaces;

namespace DataAccess.Interfaces
{
    public interface ISpecification
    {
        Task BulkInsertOrUpdate(List<Specification> entities, CancellationToken token);
    }
}
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
    public class BrandsRepository : IBrandsRepository
    {
        private readonly MasterContext _masterContext;

        public BrandsRepository(MasterContext dbContext)
        {
            _masterContext = dbContext;
        }

        public async Task<Brand> GetOneAsync(Expression<Func<Brand, bool>> predicate, CancellationToken token)
        {
            return await _masterContext.Brands.Where(predicate).FirstOrDefaultAsync(token);
        }

        public async Task InsertAsync(Brand brand, CancellationToken token)
        {
            await _masterContext.Brands.AddAsync(brand, token);
            await _masterContext.SaveChangesAsync(token);
        }
    }
}
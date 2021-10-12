using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database;
using Microsoft.EntityFrameworkCore;
using Models.Entities.RemoteApi;

namespace DataAccess.Repositories
{
    public class BrandsRep : IBrandsRep
    {
        private readonly MasterContext _masterContext;
        private readonly IGenericRep<Brand> _genBrand;

        public BrandsRep(MasterContext dbContext, IGenericRep<Brand> genBrand)
        {
            _masterContext = dbContext;
            _genBrand = genBrand;
        }

        public async Task<Brand> GetAsync(int id, CancellationToken token)
        {
            return await _genBrand.GetAsync(id, token);
        }

        public async Task<Brand> GetAsync(string slug, CancellationToken token)
        {
            return await _masterContext.Brands.Where(v => v.Slug == slug).FirstOrDefaultAsync(token);
        }

        public async Task<IEnumerable<Brand>> ListAsync(CancellationToken token)
        {
            return await _genBrand.ListAsync(token);
        }

        public async Task<IEnumerable<Brand>> ListAsync(Expression<Func<Brand, bool>> predicate,
            CancellationToken token)
        {
            return await _genBrand.ListAsync(predicate, token);
        }

        public async Task InsertAsync(Brand entity, CancellationToken token)
        {
            await _genBrand.InsertAsync(entity, token);
        }

        public async Task UpdateAsync(Brand entity, CancellationToken token)
        {
            await _genBrand.UpdateAsync(entity, token);
        }

        public async Task DeleteAsync(Brand entity, CancellationToken token)
        {
            await _genBrand.DeleteAsync(entity, token);
        }

        public async Task InsertIfNotExistsAsync(Brand entity, CancellationToken token)
        {
            var brand = await GetAsync(entity.Slug, token);
            if (brand == null)
            {
                await InsertAsync(entity, token);
            }
        }

        public async Task UpdateOrInsertAsync(Brand entity, CancellationToken token)
        {
            var brand = await GetAsync(entity.Slug, token);
            if (brand == null)
            {
                await InsertAsync(entity, token);
            }
            else
            {
                brand.Name = entity.Name;
                brand.Slug = entity.Slug;
                await UpdateAsync(brand, token);
            }
        }

        public async Task BulkInsertOrUpdate(List<Brand> entities, CancellationToken token)
        {
            await _masterContext.BulkUpdateAsync(entities,
                cancellationToken: token,
                options: options => { options.ColumnPrimaryKeyExpression = brand => brand.Slug; }
            );
            await _masterContext.BulkInsertAsync(entities,
                cancellationToken: token,
                options: options =>
                {
                    options.InsertIfNotExists = true;
                    options.ColumnPrimaryKeyExpression = brand => brand.Slug;
                }
            );
        }
    }
}
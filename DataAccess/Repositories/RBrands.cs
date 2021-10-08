using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.Entities.RemoteApi;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Repository for Models.Entities.RemoteApi.Brand
    /// </summary>
    public class RBrands : IBrands
    {
        private readonly MasterContext _masterContext;
        private readonly IGeneric<Brand> _genRBrand;

        public RBrands(MasterContext dbContext, IGeneric<Brand> genRBrand)
        {
            _masterContext = dbContext;
            _genRBrand = genRBrand;
        }

        public async Task<Brand> GetAsync(int id, CancellationToken token)
        {
            return await _genRBrand.GetAsync(id, token);
        }

        public async Task<Brand> GetAsync(string slug, CancellationToken token)
        {
            return await _masterContext.Brands.Where(v => v.Slug == slug).FirstOrDefaultAsync(token);
        }

        public async Task<IEnumerable<Brand>> ListAsync(CancellationToken token)
        {
            return await _genRBrand.ListAsync(token);
        }

        public async Task<IEnumerable<Brand>> ListAsync(Expression<Func<Brand, bool>> predicate,
            CancellationToken token)
        {
            return await _genRBrand.ListAsync(predicate, token);
        }

        public async Task InsertAsync(Brand entity, CancellationToken token)
        {
            await _genRBrand.InsertAsync(entity, token);
        }

        public async Task UpdateAsync(Brand entity, CancellationToken token)
        {
            await _genRBrand.UpdateAsync(entity, token);
        }

        public async Task DeleteAsync(Brand entity, CancellationToken token)
        {
            await _genRBrand.DeleteAsync(entity, token);
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
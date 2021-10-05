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
    /// <summary>
    /// Repository for Models.Entities.RemoteApi.Brand
    /// </summary>
    public class RBrands : IBrands
    {
        private MasterContext MasterContext { get; set; }
        private IGeneric<Brand> GenRBrand { get; set; }

        public RBrands(MasterContext dbContext, IGeneric<Brand> genRBrand)
        {
            MasterContext = dbContext;
            GenRBrand = genRBrand;
        }

        public async Task<Brand> GetAsync(int id, CancellationToken token)
        {
            return await GenRBrand.GetAsync(id, token);
        }

        public async Task<Brand> GetAsync(string slug, CancellationToken token)
        {
            return await MasterContext.Brands.Where(v => v.Slug == slug).FirstOrDefaultAsync(token);
        }

        public async Task<IEnumerable<Brand>> ListAsync(CancellationToken token)
        {
            return await GenRBrand.ListAsync(token);
        }

        public async Task<IEnumerable<Brand>> ListAsync(Expression<Func<Brand, bool>> predicate,
            CancellationToken token)
        {
            return await GenRBrand.ListAsync(predicate, token);
        }

        public async Task InsertAsync(Brand entity, CancellationToken token)
        {
            await GenRBrand.InsertAsync(entity, token);
        }

        public async Task UpdateAsync(Brand entity, CancellationToken token)
        {
            await GenRBrand.UpdateAsync(entity, token);
        }

        public async Task DeleteAsync(Brand entity, CancellationToken token)
        {
            await GenRBrand.DeleteAsync(entity, token);
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
    }
}
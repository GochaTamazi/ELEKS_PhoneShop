using System;
using System.Collections.Concurrent;
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
    /// Repository for Models.Entities.RemoteApi.Phone
    /// </summary>
    public class RPhones : IPhones
    {
        private readonly MasterContext _masterContext;
        private readonly IGeneric<Phone> _genRPhone;

        public RPhones(MasterContext dbContext, IGeneric<Phone> genRPhone)
        {
            _masterContext = dbContext;
            _genRPhone = genRPhone;
        }

        public async Task<Phone> GetAsync(int id, CancellationToken token)
        {
            return await _genRPhone.GetAsync(id, token);
        }

        public async Task<Phone> GetAsync(string slug, CancellationToken token)
        {
            return await _masterContext.PhonesRemoteApi.Where(v => v.Slug == slug).FirstOrDefaultAsync(token);
        }

        public async Task<IEnumerable<Phone>> ListAsync(CancellationToken token)
        {
            return await _genRPhone.ListAsync(token);
        }

        public async Task<IEnumerable<Phone>> ListAsync(Expression<Func<Phone, bool>> predicate,
            CancellationToken token)
        {
            return await _genRPhone.ListAsync(predicate, token);
        }

        public async Task InsertAsync(Phone entity, CancellationToken token)
        {
            await _genRPhone.InsertAsync(entity, token);
        }

        public async Task UpdateAsync(Phone entity, CancellationToken token)
        {
            await _genRPhone.UpdateAsync(entity, token);
        }

        public async Task DeleteAsync(Phone entity, CancellationToken token)
        {
            await _genRPhone.DeleteAsync(entity, token);
        }

        public async Task InsertIfNotExistsAsync(Phone entity, CancellationToken token)
        {
            var phone = await GetAsync(entity.Slug, token);
            if (phone == null)
            {
                await InsertAsync(entity, token);
            }
        }

        public async Task UpdateOrInsertAsync(Phone entity, CancellationToken token)
        {
            var phone = await GetAsync(entity.Slug, token);
            if (phone == null)
            {
                await InsertAsync(entity, token);
            }
            else
            {
                phone.BrandId = entity.BrandId;
                phone.Name = entity.Name;
                phone.Slug = entity.Slug;
                phone.Image = entity.Image;
                await UpdateAsync(phone, token);
            }
        }

        public async Task BulkInsertOrUpdate(List<Phone> entities, CancellationToken token)
        {
            await _masterContext.BulkUpdateAsync(entities,
                cancellationToken: token,
                options: options => { options.ColumnPrimaryKeyExpression = phone => phone.Slug; }
            );
            await _masterContext.BulkInsertAsync(entities,
                cancellationToken: token,
                options: options =>
                {
                    options.InsertIfNotExists = true;
                    options.ColumnPrimaryKeyExpression = phone => phone.Slug;
                }
            );
        }
    }
}
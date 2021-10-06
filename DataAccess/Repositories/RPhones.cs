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
using Models.Entities.RemoteApi;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Repository for Models.Entities.RemoteApi.Phone
    /// </summary>
    public class RPhones : IPhones
    {
        private MasterContext MasterContext { get; set; }
        private IGeneric<Phone> GenRPhone { get; set; }

        public RPhones(MasterContext dbContext, IGeneric<Phone> genRPhone)
        {
            MasterContext = dbContext;
            GenRPhone = genRPhone;
        }

        public async Task<Phone> GetAsync(int id, CancellationToken token)
        {
            return await GenRPhone.GetAsync(id, token);
        }

        public async Task<Phone> GetAsync(string slug, CancellationToken token)
        {
            return await MasterContext.PhonesRemoteApi.Where(v => v.Slug == slug).FirstOrDefaultAsync(token);
        }

        public async Task<IEnumerable<Phone>> ListAsync(CancellationToken token)
        {
            return await GenRPhone.ListAsync(token);
        }

        public async Task<IEnumerable<Phone>> ListAsync(Expression<Func<Phone, bool>> predicate,
            CancellationToken token)
        {
            return await GenRPhone.ListAsync(predicate, token);
        }

        public async Task InsertAsync(Phone entity, CancellationToken token)
        {
            await GenRPhone.InsertAsync(entity, token);
        }

        public async Task UpdateAsync(Phone entity, CancellationToken token)
        {
            await GenRPhone.UpdateAsync(entity, token);
        }

        public async Task DeleteAsync(Phone entity, CancellationToken token)
        {
            await GenRPhone.DeleteAsync(entity, token);
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
    }
}
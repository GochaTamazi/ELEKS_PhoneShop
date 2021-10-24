using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccess.Repositories
{
    public class PhonesRepository : IPhonesRepository
    {
        private readonly MasterContext _masterContext;

        public PhonesRepository(MasterContext dbContext)
        {
            _masterContext = dbContext;
        }

        public async Task<List<Phone>> GetAllAsync(CancellationToken token)
        {
            return await _masterContext.Phones.ToListAsync(token);
        }

        public async Task<List<Phone>> GetAllAsync(
            Expression<Func<Phone, bool>> condition,
            CancellationToken token)
        {
            return await _masterContext.Phones
                .Where(condition)
                .ToListAsync(token);
        }

        public async Task<List<Phone>> GetAllAsync<TKey>(
            Expression<Func<Phone, bool>> condition,
            Expression<Func<Phone, TKey>> orderBy,
            CancellationToken token)
        {
            return await _masterContext.Phones
                .Where(condition)
                .OrderBy(orderBy)
                .ToListAsync(token);
        }

        public async Task<Phone> GetOneAsync(
            Expression<Func<Phone, bool>> predicate,
            CancellationToken token)
        {
            return await _masterContext.Phones.Where(predicate).FirstOrDefaultAsync(token);
        }


        public async Task<Phone> GetPhoneBySlugAsync(string phoneSlug, CancellationToken token)
        {
            return await _masterContext.Phones.Where(phone => phone.PhoneSlug == phoneSlug).FirstOrDefaultAsync(token);
        }

        public async Task InsertAsync(Phone phone, CancellationToken token)
        {
            await _masterContext.Phones.AddAsync(phone, token);
            await _masterContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Phone phone, CancellationToken token)
        {
            _masterContext.Update(phone);
            await _masterContext.SaveChangesAsync(token);
        }

        public async Task InsertOrUpdateAsync(Phone phone, CancellationToken token)
        {
            var phoneE = await GetPhoneBySlugAsync(phone.PhoneSlug, token);
            if (phoneE == null)
            {
                await InsertAsync(phone, token);
            }
            else
            {
                _masterContext.Entry(phoneE).State = EntityState.Detached;
                phone.Id = phoneE.Id;
                await UpdateAsync(phone, token);
            }
        }

        public async Task DetachEntityAsync(Phone phone, CancellationToken token)
        {
            _masterContext.Entry(phone).State = EntityState.Detached;
        }
    }
}
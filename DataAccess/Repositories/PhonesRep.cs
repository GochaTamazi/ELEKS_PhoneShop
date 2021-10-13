using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Database;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccess.Repositories
{
    public class PhonesRep : IPhonesRep
    {
        private readonly MasterContext _masterContext;

        public PhonesRep(MasterContext dbContext)
        {
            _masterContext = dbContext;
        }

        public async Task<List<Phone>> GetAllAsync(CancellationToken token)
        {
            return await _masterContext.Phones.ToListAsync(token);
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
    }
}
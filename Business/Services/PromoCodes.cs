using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using DataAccess.Interfaces;
using Database.Models;

namespace Application.Services
{
    public class PromoCodes : IPromoCodes
    {
        private readonly IGeneralRepository<Phone> _phoneRepository;
        private readonly IGeneralRepository<PromoCode> _promoCodeRepository;

        public PromoCodes(IGeneralRepository<PromoCode> promoCodeRepository, IGeneralRepository<Phone> phoneRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _phoneRepository = phoneRepository;
        }

        public async Task<List<PromoCode>> GetAllAsync(CancellationToken token)
        {
            var codes = await _promoCodeRepository.GetAllIncludeAsync(c => c.Id > 0, c => c.Phone, token);
            if (codes != null)
            {
                return codes;
            }

            return new List<PromoCode>();
        }

        public async Task AddOrUpdateAsync(string phoneSlug, string key, int amount, int discount,
            CancellationToken token)
        {
            var phone = await _phoneRepository.GetOneAsync(phone => phone.PhoneSlug == phoneSlug && phone.Hided != true,
                token);
            if (phone != null)
            {
                var promoCode = new PromoCode()
                {
                    Key = key,
                    Amount = amount,
                    Discount = discount,
                    PhoneId = phone.Id
                };
                await _promoCodeRepository.AddOrUpdateAsync(code => code.Key == key && code.PhoneId == phone.Id,
                    promoCode,
                    token);
            }
        }

        public async Task RemoveIfExistAsync(string key, CancellationToken token)
        {
            await _promoCodeRepository.RemoveIfExistAsync(code => code.Key == key, token);
        }
    }
}
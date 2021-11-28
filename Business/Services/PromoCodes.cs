using Application.DTO.Frontend.Forms;
using Application.Interfaces;
using DataAccess.Interfaces;
using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

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

        public async Task<PromoCode> GetOneAsync(string key, CancellationToken token)
        {
            return await _promoCodeRepository.GetOneIncludeAsync(code => code.Key == key,
                code => code.Phone,
                token);
        }

        public async Task AddOrUpdateAsync(PromoCodeForm promoCodeForm, CancellationToken token)
        {
            var phone = await _phoneRepository.GetOneAsync(
                phone => phone.PhoneSlug == promoCodeForm.PhoneSlug && phone.Hided != true,
                token);
            if (phone != null)
            {
                var promoCode = new PromoCode()
                {
                    Key = promoCodeForm.Key,
                    Amount = promoCodeForm.Amount,
                    Discount = promoCodeForm.Discount,
                    PhoneId = phone.Id
                };
                await _promoCodeRepository.AddOrUpdateAsync(
                    code => code.Key == promoCodeForm.Key && code.PhoneId == phone.Id,
                    promoCode,
                    token);
            }
        }

        public async Task RemoveIfExistAsync(string key, CancellationToken token)
        {
            await _promoCodeRepository.RemoveIfExistAsync(code => code.Key == key, token);
        }

        public async Task<double> Buy(List<Cart> carts, string key, CancellationToken token)
        {
            var promoCode = await _promoCodeRepository.GetOneIncludeAsync(code => code.Key == key,
                code => code.Phone,
                token);

            double totalSumCart = 0;

            foreach (var cart in carts)
            {
                var phone = cart.Phone;
                double discount = 1;
                if (promoCode != null && phone.PhoneSlug == promoCode.Phone.PhoneSlug)
                {
                    discount = (double) ((100.0 - promoCode.Discount) / 100.0);

                    promoCode.Amount--;
                    await _promoCodeRepository.UpdateAsync(promoCode, token);
                }

                var totalPrice = cart.Amount * phone.Price * discount ?? 0;
                totalSumCart += totalPrice;
            }

            return totalSumCart;
        }
    }
}
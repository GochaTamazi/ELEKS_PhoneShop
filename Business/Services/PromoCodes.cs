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
        private readonly IGeneralRepository<PromoCode> _promoCodeRepository;

        public PromoCodes(IGeneralRepository<PromoCode> promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
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
    }
}
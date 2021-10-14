using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Services
{
    public class CustomerPhones : ICustomerPhones
    {
        private readonly IBrandsRep _brandsRep;
        private readonly IPhonesRep _phonesRep;
        private readonly IMapperProvider _mapperProvider;

        public CustomerPhones(IBrandsRep brandsRep, IPhonesRep phonesRep, IMapperProvider mapperProvider)
        {
            _brandsRep = brandsRep;
            _phonesRep = phonesRep;
            _mapperProvider = mapperProvider;
        }

        public async Task<List<Phone>> GetPhonesAsync(PhonesFilter filter, CancellationToken token)
        {
            Expression<Func<Phone, bool>> condition = (phone) =>
                EF.Functions.Like(phone.BrandSlug, $"%{filter.BrandName}%")
                &&
                EF.Functions.Like(phone.PhoneName, $"%{filter.PhoneName}%")
                &&
                filter.PriceMin <= phone.Price && phone.Price <= filter.PriceMax
                &&
                ((!filter.InStock) || 1 <= phone.Stock)
                &&
                phone.Hided == false;

            return await _phonesRep.GetAllAsync(condition, token);
        }

        public async Task<DTO.Frontend.PhoneDto> GetPhoneAsync(string phoneSlug, CancellationToken token)
        {
            Expression<Func<Phone, bool>> condition = (phone) =>
                phone.PhoneSlug == phoneSlug
                &&
                phone.Hided == false;

            var phoneE = await _phonesRep.GetOneAsync(condition, token);

            return phoneE == null ? new PhoneDto() : _mapperProvider.GetMapper().Map<PhoneDto>(phoneE);
        }
    }
}
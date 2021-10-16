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
        private readonly IPriceSubscribersRep _priceSubsRep;
        private readonly IStockSubscribersRep _stockSubsRep;

        public CustomerPhones(IBrandsRep brandsRep, IPhonesRep phonesRep, IMapperProvider mapperProvider,
            IPriceSubscribersRep priceSubsRep, IStockSubscribersRep stockSubsRep)
        {
            _brandsRep = brandsRep;
            _phonesRep = phonesRep;
            _mapperProvider = mapperProvider;
            _priceSubsRep = priceSubsRep;
            _stockSubsRep = stockSubsRep;
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

        public async Task SubscribePriceAsync(PriceSubscriberFront priceSubscriber, CancellationToken token)
        {
            var priceSubs = _mapperProvider.GetMapper().Map<PriceSubscriber>(priceSubscriber);

            var subs = await _priceSubsRep.GetOneAsync(subs =>
                    subs.BrandSlug == priceSubs.BrandSlug &&
                    subs.PhoneSlug == priceSubs.PhoneSlug &&
                    subs.Email == priceSubs.Email,
                token);

            if (subs == null)
            {
                await _priceSubsRep.InsertAsync(priceSubs, token);
            }
        }

        public async Task SubscribeStockAsync(StockSubscriberFront stockSubscriber, CancellationToken token)
        {
            var stockSubs = _mapperProvider.GetMapper().Map<StockSubscriber>(stockSubscriber);

            var subs = await _stockSubsRep.GetOneAsync(subs =>
                    subs.BrandSlug == stockSubs.BrandSlug &&
                    subs.PhoneSlug == stockSubs.PhoneSlug &&
                    subs.Email == stockSubs.Email,
                token);

            if (subs == null)
            {
                await _stockSubsRep.InsertAsync(stockSubs, token);
            }
        }
    }
}
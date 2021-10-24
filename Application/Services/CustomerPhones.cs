using Application.DTO.Frontend;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using PagedList;

namespace Application.Services
{
    public class CustomerPhones : ICustomerPhones
    {
        private readonly IBrandsRepository _brandsRepository;
        private readonly IMapperProvider _mapperProvider;
        private readonly IPhonesRepository _phonesRepository;
        private readonly IPriceSubscribersRepository _priceSubscribersRepository;
        private readonly IStockSubscribersRepository _stockSubscribersRepository;

        public CustomerPhones(
            IBrandsRepository brandsRepository,
            IMapperProvider mapperProvider,
            IPhonesRepository phonesRepository,
            IPriceSubscribersRepository priceSubscribersRepository,
            IStockSubscribersRepository stockSubscribersRepository
        )
        {
            _brandsRepository = brandsRepository;
            _mapperProvider = mapperProvider;
            _phonesRepository = phonesRepository;
            _priceSubscribersRepository = priceSubscribersRepository;
            _stockSubscribersRepository = stockSubscribersRepository;
        }

        /// <summary>
        /// Subscription of a customer to change the price for a specific phone.
        /// </summary>
        public async Task SubscribePriceAsync(PriceSubscriberFront priceSubscriberFront, CancellationToken token)
        {
            var priceSubscriber = _mapperProvider.GetMapper().Map<PriceSubscriber>(priceSubscriberFront);

            var priceSubscriberModel = await _priceSubscribersRepository.GetOneAsync(sub =>
                    sub.BrandSlug == priceSubscriber.BrandSlug &&
                    sub.PhoneSlug == priceSubscriber.PhoneSlug &&
                    sub.Email == priceSubscriber.Email,
                token);

            if (priceSubscriberModel == null)
            {
                await _priceSubscribersRepository.InsertAsync(priceSubscriber, token);
            }
        }

        /// <summary>
        /// Subscription of a customer to change the stock for a specific phone.
        /// </summary>
        public async Task SubscribeStockAsync(StockSubscriberFront stockSubscriberFront, CancellationToken token)
        {
            var stockSubscriber = _mapperProvider.GetMapper().Map<StockSubscriber>(stockSubscriberFront);

            var stockSubscriberModel = await _stockSubscribersRepository.GetOneAsync(sub =>
                    sub.BrandSlug == stockSubscriber.BrandSlug &&
                    sub.PhoneSlug == stockSubscriber.PhoneSlug &&
                    sub.Email == stockSubscriber.Email,
                token);

            if (stockSubscriberModel == null)
            {
                await _stockSubscribersRepository.InsertAsync(stockSubscriber, token);
            }
        }

        /// <summary>
        /// Get phones by specified filter. 
        /// </summary>
        public async Task<PhonesPageFront> GetPhonesAsync(
            PhonesFilter filter,
            int page,
            int pageSize,
            CancellationToken token
        )
        {
            Expression<Func<Phone, bool>> condition = (phone) =>
                EF.Functions.Like(phone.BrandSlug, $"%{filter.BrandName}%") &&
                EF.Functions.Like(phone.PhoneName, $"%{filter.PhoneName}%") &&
                filter.PriceMin <= phone.Price && phone.Price <= filter.PriceMax &&
                ((!filter.InStock) || 1 <= phone.Stock) &&
                phone.Hided == false;

            Expression<Func<Phone, object>> orderBy;
            switch (filter.OrderBy)
            {
                default:
                    orderBy = (phone) => phone.PhoneName;
                    break;
                case "PhoneName":
                    orderBy = (phone) => phone.PhoneName;
                    break;
                case "BrandSlug":
                    orderBy = (phone) => phone.BrandSlug;
                    break;
                case "Price":
                    orderBy = (phone) => phone.Price;
                    break;
                case "Stock":
                    orderBy = (phone) => phone.Stock;
                    break;
            }

            var phones = await _phonesRepository.GetAllAsync(
                condition,
                orderBy,
                token);

            var totalPages = (int) Math.Ceiling((double) phones.Count / pageSize);

            if (page <= 0)
            {
                page = 1;
            }

            return new PhonesPageFront()
            {
                TotalPhones = phones.Count,
                TotalPages = totalPages,
                PageSize = pageSize,
                Page = page,
                Phones = phones.ToPagedList(page, pageSize).ToList()
            };
        }

        /// <summary>
        /// Get phone by phoneSlug.
        /// </summary>
        public async Task<DTO.Frontend.PhoneDto> GetPhoneAsync(string phoneSlug, CancellationToken token)
        {
            Expression<Func<Phone, bool>> condition = (phone) =>
                phone.PhoneSlug == phoneSlug &&
                phone.Hided == false;

            var phoneModel = await _phonesRepository.GetOneAsync(condition, token);

            return phoneModel == null ? new PhoneDto() : _mapperProvider.GetMapper().Map<PhoneDto>(phoneModel);
        }
    }
}
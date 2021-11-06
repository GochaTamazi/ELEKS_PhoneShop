using Application.DTO.Frontend.Forms;
using Application.DTO.Frontend;
using Application.Interfaces;
using DataAccess.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;

namespace Application.Services
{
    public class AdminPhones : IAdminPhones
    {
        private readonly IGeneralRepository<Brand> _brandsRepository;
        private readonly IGeneralRepository<Phone> _phonesRepository;
        private readonly IGeneralRepository<PriceSubscriber> _priceSubscribersRepository;
        private readonly IGeneralRepository<StockSubscriber> _stockSubscribersRepository;
        private readonly IGeneralRepository<WishList> _wishListRepository;
        private readonly IMailNotification _mailNotification;
        private readonly IMapperProvider _mapperProvider;
        private readonly IPhoneSpecificationsApi _phoneSpecificationServiceApi;

        public AdminPhones(
            IGeneralRepository<Brand> brandsRepository,
            IGeneralRepository<Phone> phonesRepository,
            IGeneralRepository<PriceSubscriber> priceSubscribersRepository,
            IGeneralRepository<StockSubscriber> stockSubscribersRepository,
            IGeneralRepository<WishList> wishListRepository,
            IMailNotification mailNotification,
            IMapperProvider mapperProvider,
            IPhoneSpecificationsApi phoneSpecificationServiceApi
        )
        {
            _brandsRepository = brandsRepository;
            _phonesRepository = phonesRepository;
            _priceSubscribersRepository = priceSubscribersRepository;
            _stockSubscribersRepository = stockSubscribersRepository;
            _wishListRepository = wishListRepository;
            _mailNotification = mailNotification;
            _mapperProvider = mapperProvider;
            _phoneSpecificationServiceApi = phoneSpecificationServiceApi;
        }

        public async Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token)
        {
            var phoneSpecificationsDto = await _phoneSpecificationServiceApi.GetPhoneSpecificationsOrThrowAsync(
                phoneSpecFront.PhoneSlug, token);

            var phoneFromApi = _mapperProvider.GetMapper().Map<PhoneSpecificationsDto, Phone>(phoneSpecificationsDto);
            phoneFromApi = _mapperProvider.GetMapper().Map<PhoneSpecFront, Phone>(phoneSpecFront, phoneFromApi);

            var phoneFromDb = await _phonesRepository.InsertOrUpdateAsync(phone =>
                phone.PhoneSlug == phoneFromApi.PhoneSlug, phoneFromApi, token);

            //Price notification
            if (phoneFromApi.Price != phoneFromDb.Price)
            {
                await PriceSubscribersNotificationAsync(phoneFromApi, token);
                await PriceWishListCustomerNotificationAsync(phoneFromDb, token);
            }

            //Stock notification
            if (phoneFromApi.Stock != phoneFromDb.Stock && phoneFromDb.Stock <= 0)
            {
                await StockSubscribersNotificationAsync(phoneFromApi, token);
            }

            await BrandInsertIfNotExistAsync(phoneFromApi.BrandSlug, token);
        }

        public async Task<PhoneSpecFront> GetPhoneAsync(string phoneSlug, CancellationToken token)
        {
            var phoneSpecificationsDto =
                await _phoneSpecificationServiceApi.GetPhoneSpecificationsOrThrowAsync(phoneSlug, token);

            var listBrands = await _phoneSpecificationServiceApi.GetListBrandsOrThrowAsync(token);
            var brand = listBrands.Data.FirstOrDefault(brand => brand.Brand_name == phoneSpecificationsDto.Data.Brand);

            var phoneSpecFront = new PhoneSpecFront()
            {
                PhoneSlug = phoneSlug,
                BrandSlug = brand.Brand_slug,
            };

            var phone = await _phonesRepository.GetOneAsync(phone => phone.PhoneSlug == phoneSlug, token);

            if (phone != null)
            {
                phoneSpecFront = _mapperProvider.GetMapper().Map<Phone, PhoneSpecFront>(phone);
                phoneSpecFront.InStore = true;
            }

            phoneSpecFront.PhoneDetail = phoneSpecificationsDto.Data;

            return phoneSpecFront;
        }

        public async Task<PhonesPageFront> GetPhonesAsync(PhonesFilterForm filterForm, int page, int pageSize,
            CancellationToken token)
        {
            Expression<Func<Phone, bool>> condition = (phone) =>
                EF.Functions.Like(phone.BrandSlug, $"%{filterForm.BrandName}%") &&
                EF.Functions.Like(phone.PhoneName, $"%{filterForm.PhoneName}%") &&
                filterForm.PriceMin <= phone.Price && phone.Price <= filterForm.PriceMax &&
                ((!filterForm.InStock) || 1 <= phone.Stock);

            Expression<Func<Phone, object>> orderBy = filterForm.OrderBy switch
            {
                "PhoneName" => (phone) => phone.PhoneName,
                "BrandSlug" => (phone) => phone.BrandSlug,
                "Price" => (phone) => phone.Price,
                "Stock" => (phone) => phone.Stock,
                _ => (phone) => phone.PhoneName
            };

            var phones = await _phonesRepository.GetAllAsync(condition, orderBy, token);

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

        private async Task BrandInsertIfNotExistAsync(string brandSlug, CancellationToken token)
        {
            var brandModelFromDb = await _brandsRepository.GetOneAsync(brand => brand.Slug == brandSlug, token);
            if (brandModelFromDb == null)
            {
                var listBrandsDto = await _phoneSpecificationServiceApi.GetListBrandsOrThrowAsync(token);
                var brandDto = listBrandsDto.Data.FirstOrDefault(brandDto => brandDto.Brand_slug == brandSlug);
                var brandModelFromApi = _mapperProvider.GetMapper().Map<Brand>(brandDto);
                await _brandsRepository.InsertAsync(brandModelFromApi, token);
            }
        }

        private async Task PriceSubscribersNotificationAsync(Phone phone, CancellationToken token)
        {
            var subscribers = await _priceSubscribersRepository
                .GetAllAsync(subscriber =>
                    subscriber.BrandSlug == phone.BrandSlug &&
                    subscriber.PhoneSlug == phone.PhoneSlug, token);
            await _mailNotification.PriceSubscribersNotificationAsync(subscribers, phone, token);
        }

        private async Task PriceWishListCustomerNotificationAsync(Phone phone, CancellationToken token)
        {
            var wishList = await _wishListRepository.GetAllIncludeAsync(
                list => list.PhoneId == phone.Id,
                list => list.User,
                token);
            await _mailNotification.PriceWishListCustomerNotificationAsync(wishList, phone, token);
        }

        private async Task StockSubscribersNotificationAsync(Phone phone, CancellationToken token)
        {
            var subscribers = await _stockSubscribersRepository
                .GetAllAsync(subscriber =>
                    subscriber.BrandSlug == phone.BrandSlug &&
                    subscriber.PhoneSlug == phone.PhoneSlug, token);
            await _mailNotification.StockSubscribersNotificationAsync(subscribers, phone, token);
        }
    }
}
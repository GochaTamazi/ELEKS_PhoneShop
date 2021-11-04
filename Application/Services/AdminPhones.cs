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

        public async Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront,
            CancellationToken token)
        {
            var phoneSpecificationsDto = await _phoneSpecificationServiceApi.GetPhoneSpecificationsOrThrowAsync(
                phoneSpecFront.PhoneSlug, token);

            var phoneModelFromApi = _mapperProvider.GetMapper().Map<Phone>(phoneSpecificationsDto);
            phoneModelFromApi.BrandSlug = phoneSpecFront.BrandSlug;
            phoneModelFromApi.PhoneSlug = phoneSpecFront.PhoneSlug;
            phoneModelFromApi.Price = phoneSpecFront.Price;
            phoneModelFromApi.Stock = phoneSpecFront.Stock;
            phoneModelFromApi.Hided = phoneSpecFront.Hided;

            var phoneModelFromDb = await _phonesRepository.GetOneAsync(phone =>
                phone.PhoneSlug == phoneModelFromApi.PhoneSlug, token);
            if (phoneModelFromDb == null)
            {
                await _phonesRepository.InsertAsync(phoneModelFromApi, token);
            }
            else
            {
                _phonesRepository.DetachEntity(phoneModelFromDb);
                phoneModelFromApi.Id = phoneModelFromDb.Id;
                await _phonesRepository.UpdateAsync(phoneModelFromApi, token);

                //Price notification
                if (phoneModelFromApi.Price != phoneModelFromDb.Price)
                {
                    await PriceSubscribersNotificationAsync(phoneModelFromApi, token);
                    await PriceWishListCustomerNotificationAsync(phoneModelFromDb, token);
                }

                //Stock notification
                if (phoneModelFromApi.Stock != phoneModelFromDb.Stock && phoneModelFromDb.Stock <= 0)
                {
                    await StockSubscribersNotificationAsync(phoneModelFromApi, token);
                }
            }

            await BrandInsertIfNotExistAsync(phoneModelFromApi.BrandSlug, token);
        }

        public async Task<PhoneSpecFront> GetPhoneAsync(string phoneSlug,
            CancellationToken token)
        {
            var phoneSpecificationsDto = await _phoneSpecificationServiceApi.GetPhoneSpecificationsOrThrowAsync(
                phoneSlug, token);

            var phoneSpecFront = new PhoneSpecFront()
            {
                PhoneDetail = phoneSpecificationsDto.Data,
                PhoneSlug = phoneSlug
            };

            var phoneModel = await _phonesRepository.GetOneAsync(phone => phone.PhoneSlug == phoneSlug, token);
            if (phoneModel != null)
            {
                phoneSpecFront.InStore = true;
                phoneSpecFront.BrandSlug = phoneModel.BrandSlug;
                phoneSpecFront.Price = phoneModel.Price;
                phoneSpecFront.Stock = phoneModel.Stock;
                if (phoneModel.Hided != null)
                {
                    phoneSpecFront.Hided = (bool) phoneModel.Hided;
                }
            }
            else
            {
                var listBrandsDto = await _phoneSpecificationServiceApi.GetListBrandsOrThrowAsync(token);

                var brandDto = listBrandsDto.Data.FirstOrDefault(brandDto =>
                    brandDto.Brand_name == phoneSpecificationsDto.Data.Brand);

                if (brandDto != null)
                {
                    phoneSpecFront.BrandSlug = brandDto.Brand_slug;
                }
            }

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

            Expression<Func<Phone, object>> orderBy;
            switch (filterForm.OrderBy)
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


        private async Task BrandInsertIfNotExistAsync(string brandSlug,
            CancellationToken token)
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

        private async Task PriceSubscribersNotificationAsync(Phone phone,
            CancellationToken token)
        {
            var subscribers = await _priceSubscribersRepository
                .GetAllAsync(subscriber =>
                    subscriber.BrandSlug == phone.BrandSlug &&
                    subscriber.PhoneSlug == phone.PhoneSlug, token);
            await _mailNotification.PriceSubscribersNotificationAsync(subscribers, phone, token);
        }

        private async Task PriceWishListCustomerNotificationAsync(Phone phone,
            CancellationToken token)
        {
            var wishList = await _wishListRepository.GetAllIncludeAsync(
                list => list.PhoneId == phone.Id,
                list => list.User,
                token);
            await _mailNotification.PriceWishListCustomerNotificationAsync(wishList, phone, token);
        }

        private async Task StockSubscribersNotificationAsync(Phone phone,
            CancellationToken token)
        {
            var subscribers = await _stockSubscribersRepository
                .GetAllAsync(subscriber =>
                    subscriber.BrandSlug == phone.BrandSlug &&
                    subscriber.PhoneSlug == phone.PhoneSlug, token);
            await _mailNotification.StockSubscribersNotificationAsync(subscribers, phone, token);
        }
    }
}
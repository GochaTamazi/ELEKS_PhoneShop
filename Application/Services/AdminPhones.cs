using Application.DTO.Frontend;
using Application.Interfaces;
using DataAccess.Interfaces;
using Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using PagedList;

namespace Application.Services
{
    public class AdminPhones : IAdminPhones
    {
        private readonly IBrandsRepository _brandsRepository;
        private readonly IMailNotification _mailNotification;
        private readonly IMapperProvider _mapperProvider;
        private readonly IPhoneSpecificationsApi _phoneSpecificationServiceApi;
        private readonly IPhonesRepository _phonesRepository;
        private readonly IPriceSubscribersRepository _priceSubscribersRepository;
        private readonly IStockSubscribersRepository _stockSubscribersRepository;

        public AdminPhones(
            IBrandsRepository brandsRepository,
            IMailNotification mailNotification,
            IMapperProvider mapperProvider,
            IPhoneSpecificationsApi phoneSpecification,
            IPhonesRepository phonesRepository,
            IPriceSubscribersRepository priceSubscribersRepository,
            IStockSubscribersRepository stockSubscribersRepository
        )
        {
            _brandsRepository = brandsRepository;
            _mailNotification = mailNotification;
            _mapperProvider = mapperProvider;
            _phoneSpecificationServiceApi = phoneSpecification;
            _phonesRepository = phonesRepository;
            _priceSubscribersRepository = priceSubscribersRepository;
            _stockSubscribersRepository = stockSubscribersRepository;
        }

        /// <summary>
        /// Add a new brand if it is not in the database.
        /// </summary>
        public async Task BrandInsertIfNotExistAsync(string brandSlug, CancellationToken token)
        {
            var brandModelFromDb = await _brandsRepository.GetBySlugAsync(brandSlug, token);
            if (brandModelFromDb == null)
            {
                var listBrandsDto = await _phoneSpecificationServiceApi.ListBrandsAsync(token);
                if (listBrandsDto.Status == false)
                {
                    throw new Exception("PhoneSpecificationsApi not responds");
                }

                var brandDto = listBrandsDto.Data.FirstOrDefault(brandDto => brandDto.Brand_slug == brandSlug);
                var brandModelFromApi = _mapperProvider.GetMapper().Map<Brand>(brandDto);
                await _brandsRepository.InsertAsync(brandModelFromApi, token);
            }
        }

        /// <summary>
        /// Update or add information about the specified phone.
        /// </summary>
        public async Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token)
        {
            var phoneSpecificationsDto = await _phoneSpecificationServiceApi.PhoneSpecificationsAsync(
                phoneSpecFront.PhoneSlug,
                token);
            if (phoneSpecificationsDto.Status == false)
            {
                throw new Exception("PhoneSpecificationsApi not responds");
            }

            var phoneModelFromApi = _mapperProvider.GetMapper().Map<Phone>(phoneSpecificationsDto);
            phoneModelFromApi.BrandSlug = phoneSpecFront.BrandSlug;
            phoneModelFromApi.PhoneSlug = phoneSpecFront.PhoneSlug;
            phoneModelFromApi.Price = phoneSpecFront.Price;
            phoneModelFromApi.Stock = phoneSpecFront.Stock;
            phoneModelFromApi.Hided = phoneSpecFront.Hided;

            var phoneModelFromDb = await _phonesRepository.GetPhoneBySlugAsync(phoneModelFromApi.PhoneSlug, token);
            if (phoneModelFromDb == null)
            {
                await _phonesRepository.InsertAsync(phoneModelFromApi, token);
            }
            else
            {
                await _phonesRepository.DetachEntityAsync(phoneModelFromDb, token);
                phoneModelFromApi.Id = phoneModelFromDb.Id;
                await _phonesRepository.UpdateAsync(phoneModelFromApi, token);
                if (phoneModelFromApi.Price != phoneModelFromDb.Price)
                {
                    //Price notification
                    await PriceSubscribersNotificationAsync(phoneModelFromApi, token);
                }

                if (phoneModelFromApi.Stock != phoneModelFromDb.Stock && phoneModelFromDb.Stock <= 0)
                {
                    //Stock notification
                    await StockSubscribersNotificationAsync(phoneModelFromApi, token);
                }
            }

            await BrandInsertIfNotExistAsync(phoneModelFromApi.BrandSlug, token);
        }

        /// <summary>
        /// Price change notification to subscribers.
        /// </summary>
        public async Task PriceSubscribersNotificationAsync(Phone phone, CancellationToken token)
        {
            var subscribers = await _priceSubscribersRepository
                .GetAllAsync(subscriber =>
                    subscriber.BrandSlug == phone.BrandSlug &&
                    subscriber.PhoneSlug == phone.PhoneSlug, token);
            await _mailNotification.PriceSubscribersNotificationAsync(subscribers, phone, token);
        }

        /// <summary>
        /// Stock change notification to subscribers.
        /// </summary>
        public async Task StockSubscribersNotificationAsync(Phone phone, CancellationToken token)
        {
            var subscribers = await _stockSubscribersRepository
                .GetAllAsync(subscriber =>
                    subscriber.BrandSlug == phone.BrandSlug &&
                    subscriber.PhoneSlug == phone.PhoneSlug, token);
            await _mailNotification.StockSubscribersNotificationAsync(subscribers, phone, token);
        }

        /// <summary>
        /// Get all phones that are added to the store. Including hidden.
        /// </summary>
        public async Task<PhonesPageFront> GetPhonesInStoreAsync(int page, int pageSize, CancellationToken token)
        {
            var phones = await _phonesRepository.GetAllAsync(token);
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
        /// Get information about the phone from the remote api. Or from a local database if one already exists in the store.
        /// </summary>
        public async Task<PhoneSpecFront> GetPhone(string phoneSlug, CancellationToken token)
        {
            var phoneSpecificationsDto = await _phoneSpecificationServiceApi.PhoneSpecificationsAsync(phoneSlug, token);
            if (phoneSpecificationsDto.Status == false)
            {
                throw new Exception("PhoneSpecificationsApi not responds");
            }

            var phoneSpecFront = new PhoneSpecFront()
            {
                PhoneDetail = phoneSpecificationsDto.Data,
                PhoneSlug = phoneSlug
            };

            var phoneModel = await _phonesRepository.GetPhoneBySlugAsync(phoneSlug, token);
            if (phoneModel != null)
            {
                phoneSpecFront.InStore = true;
                phoneSpecFront.BrandSlug = phoneModel.BrandSlug;
                phoneSpecFront.Price = phoneModel.Price;
                phoneSpecFront.Stock = phoneModel.Stock;
                phoneSpecFront.Hided = (bool) phoneModel.Hided;
            }
            else
            {
                var listBrandsDto = await _phoneSpecificationServiceApi.ListBrandsAsync(token);
                if (listBrandsDto.Status == false)
                {
                    throw new Exception("PhoneSpecificationsApi not responds");
                }

                var brandDto = listBrandsDto.Data
                    .FirstOrDefault(brandDto => brandDto.Brand_name == phoneSpecificationsDto.Data.Brand);

                if (brandDto != null)
                {
                    phoneSpecFront.BrandSlug = brandDto.Brand_slug;
                }
            }

            return phoneSpecFront;
        }
    }
}
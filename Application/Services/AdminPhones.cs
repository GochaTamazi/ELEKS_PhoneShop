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
        private readonly IMailNotification _mailNotification;
        private readonly IMapperProvider _mapperProvider;
        private readonly IPhoneSpecificationsApi _phoneSpecificationServiceApi;

        public AdminPhones(
            IGeneralRepository<Brand> brandsRepository,
            IGeneralRepository<Phone> phonesRepository,
            IMailNotification mailNotification,
            IMapperProvider mapperProvider,
            IPhoneSpecificationsApi phoneSpecificationServiceApi
        )
        {
            _brandsRepository = brandsRepository;
            _phonesRepository = phonesRepository;
            _mailNotification = mailNotification;
            _mapperProvider = mapperProvider;
            _phoneSpecificationServiceApi = phoneSpecificationServiceApi;
        }

        public async Task<Phone> PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token)
        {
            var phoneSpecificationsDto = await _phoneSpecificationServiceApi.GetPhoneSpecificationsAsync(
                phoneSpecFront.PhoneSlug, token);

            if (phoneSpecificationsDto == null)
            {
                return null;
            }

            var phoneFromApi = _mapperProvider.GetMapper().Map<PhoneSpecificationsDto, Phone>(phoneSpecificationsDto);
            phoneFromApi = _mapperProvider.GetMapper().Map<PhoneSpecFront, Phone>(phoneSpecFront, phoneFromApi);

            var phoneFromDb = await _phonesRepository.InsertOrUpdateAsync(phone =>
                phone.PhoneSlug == phoneFromApi.PhoneSlug, phoneFromApi, token);

            //Price notification
            if (phoneFromApi.Price != phoneFromDb.Price)
            {
                await _mailNotification.PriceSubscribersNotificationAsync(phoneFromApi, token);
                await _mailNotification.PriceWishListCustomerNotificationAsync(phoneFromApi, token);
            }

            //Stock notification
            if (phoneFromApi.Stock != phoneFromDb.Stock && phoneFromDb.Stock <= 0)
            {
                await _mailNotification.StockSubscribersNotificationAsync(phoneFromApi, token);
            }

            await BrandInsertIfNotExistAsync(phoneFromApi.BrandSlug, token);

            return phoneFromApi;
        }

        public async Task<PhoneSpecFront> GetPhoneAsync(string phoneSlug, CancellationToken token)
        {
            var phoneSpecificationsDto = await _phoneSpecificationServiceApi.GetPhoneSpecificationsAsync(
                phoneSlug, token);

            var listBrands = await _phoneSpecificationServiceApi.GetListBrandsAsync(token);

            if (phoneSpecificationsDto == null || listBrands == null)
            {
                return null;
            }

            var brand = listBrands.Data.FirstOrDefault(brand => brand.Brand_name == phoneSpecificationsDto.Data.Brand);

            var phoneSpecFront = new PhoneSpecFront()
            {
                PhoneSlug = phoneSlug,
                BrandSlug = brand?.Brand_slug
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

            if (page < 1)
            {
                page = 1;
            }

            return new PhonesPageFront()
            {
                TotalPhones = phones.Count,
                TotalPages = (int) Math.Ceiling((double) phones.Count / pageSize),
                PageSize = pageSize,
                Page = page,
                Phones = phones.ToPagedList(page, pageSize).ToList()
            };
        }

        private async Task BrandInsertIfNotExistAsync(string brandSlug, CancellationToken token)
        {
            var listBrandsDto = await _phoneSpecificationServiceApi.GetListBrandsAsync(token);
            var brandDto = listBrandsDto?.Data.FirstOrDefault(brandDto => brandDto.Brand_slug == brandSlug);
            var brandModelFromApi = _mapperProvider.GetMapper().Map<Brand>(brandDto);
            await _brandsRepository.InsertIfNotExistAsync(b => b.Slug == brandSlug, brandModelFromApi, token);
        }
    }
}
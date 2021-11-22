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
using System.Collections.Generic;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using AutoMapper;

namespace Application.Services
{
    public class AdminPhones : IAdminPhones
    {
        private readonly IGeneralRepository<Brand> _brandsRepository;
        private readonly IGeneralRepository<Phone> _phonesRepository;
        private readonly IMailNotification _mailNotification;
        private readonly IPhoneSpecificationsApi _phoneSpecificationServiceApi;
        private readonly IMapper _mapper;

        public AdminPhones(
            IGeneralRepository<Brand> brandsRepository,
            IGeneralRepository<Phone> phonesRepository,
            IMailNotification mailNotification,
            IPhoneSpecificationsApi phoneSpecificationServiceApi,
            IMapper mapper
        )
        {
            _brandsRepository = brandsRepository;
            _phonesRepository = phonesRepository;
            _mailNotification = mailNotification;
            _phoneSpecificationServiceApi = phoneSpecificationServiceApi;
            _mapper = mapper;
        }

        public async Task<Phone> AddOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token)
        {
            var phoneSpecificationsDto = await _phoneSpecificationServiceApi.GetPhoneSpecificationsAsync(
                phoneSpecFront.PhoneSlug, token);

            if (phoneSpecificationsDto == null)
            {
                return null;
            }

            var phoneFromApi = _mapper.Map<PhoneSpecificationsDto, Phone>(phoneSpecificationsDto);
            phoneFromApi = _mapper.Map<PhoneSpecFront, Phone>(phoneSpecFront, phoneFromApi);

            var phoneFromDb = await _phonesRepository.AddOrUpdateAsync(phone =>
                phone.PhoneSlug == phoneFromApi.PhoneSlug, phoneFromApi, token);

            //Price notification
            if (phoneFromApi.Price != phoneFromDb.Price)
            {
                await _mailNotification.NotifyPriceSubscribersAsync(phoneFromApi, token);
                await _mailNotification.NotifyPriceWishListCustomerAsync(phoneFromApi, token);
            }

            //Stock notification
            if (phoneFromApi.Stock != phoneFromDb.Stock && phoneFromDb.Stock <= 0)
            {
                await _mailNotification.NotifyStockSubscribersAsync(phoneFromApi, token);
            }

            await BrandInsertIfNotExistAsync(phoneFromApi.BrandSlug, token);

            return phoneFromApi;
        }

        public async Task AddOrUpdateAsync(List<Phone> phones, CancellationToken token)
        {
            foreach (var phone in phones)
            {
                var phoneDb = await _phonesRepository.AddOrUpdateAsync(p => p.PhoneSlug == phone.PhoneSlug,
                    phone, token);

                //Price notification
                if (phone.Price != phoneDb.Price)
                {
                    await _mailNotification.NotifyPriceSubscribersAsync(phone, token);
                    await _mailNotification.NotifyPriceWishListCustomerAsync(phone, token);
                }

                //Stock notification
                if (phone.Stock != phoneDb.Stock && phoneDb.Stock <= 0)
                {
                    await _mailNotification.NotifyStockSubscribersAsync(phone, token);
                }
            }
        }

        public async Task<PhoneSpecFront> GetOneAsync(string phoneSlug, CancellationToken token)
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
                phoneSpecFront = _mapper.Map<Phone, PhoneSpecFront>(phone);
                phoneSpecFront.InStore = true;
            }

            phoneSpecFront.PhoneDetail = phoneSpecificationsDto.Data;

            return phoneSpecFront;
        }

        public async Task<List<Phone>> GetAllAsync(PhonesFilterForm filterForm, CancellationToken token)
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

            return phones;
        }

        public async Task<PhonesPageFront> GetAllPagedAsync(PhonesFilterForm filterForm, int page, int pageSize,
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
            var brandModelFromApi = _mapper.Map<Brand>(brandDto);
            await _brandsRepository.AddIfNotExistAsync(b => b.Slug == brandSlug, brandModelFromApi, token);
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend;
using Application.Interfaces;
using DataAccess.Interfaces;
using Models.Entities;

namespace Application.Services
{
    public class AdminPhones : IAdminPhones
    {
        private readonly IPhoneSpecificationsApi _phoneSpecificationServiceApi;
        private readonly IBrandsRep _brandsRep;
        private readonly IPhonesRep _phonesRep;
        private readonly IMapperProvider _mapperProvider;

        public AdminPhones(IPhoneSpecificationsApi phoneSpecification, IBrandsRep brandsRep, IPhonesRep phonesRep,
            IMapperProvider mapperProvider)
        {
            _phoneSpecificationServiceApi = phoneSpecification;
            _brandsRep = brandsRep;
            _phonesRep = phonesRep;
            _mapperProvider = mapperProvider;
        }

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
            var phoneE = await _phonesRep.GetPhoneBySlugAsync(phoneSlug, token);
            if (phoneE != null)
            {
                phoneSpecFront.InStore = true;
                phoneSpecFront.BrandSlug = phoneE.BrandSlug;
                if (phoneE.Price != null) phoneSpecFront.Price = (int) phoneE.Price;
                if (phoneE.Stock != null) phoneSpecFront.Stock = (int) phoneE.Stock;
                if (phoneE.Hided != null) phoneSpecFront.Hided = (bool) phoneE.Hided;
            }
            else
            {
                var brands = await _phoneSpecificationServiceApi.ListBrandsAsync(token);
                if (brands.Status == false)
                {
                    throw new Exception("PhoneSpecificationsApi not responds");
                }

                var brandDto = brands.Data.FirstOrDefault(b => b.Brand_name == phoneSpecificationsDto.Data.Brand);
                if (brandDto != null) phoneSpecFront.BrandSlug = brandDto.Brand_slug;
            }

            return phoneSpecFront;
        }

        public async Task PhoneInsertOrUpdateAsync(PhoneSpecFront phoneSpecFront, CancellationToken token)
        {
            var phoneSpecificationsDto =
                await _phoneSpecificationServiceApi.PhoneSpecificationsAsync(phoneSpecFront.PhoneSlug, token);
            if (phoneSpecificationsDto.Status == false)
            {
                throw new Exception("PhoneSpecificationsApi not responds");
            }

            var phone = _mapperProvider.GetMapper().Map<Phone>(phoneSpecificationsDto);
            phone.BrandSlug = phoneSpecFront.BrandSlug;
            phone.PhoneSlug = phoneSpecFront.PhoneSlug;
            phone.Price = phoneSpecFront.Price;
            phone.Stock = phoneSpecFront.Stock;
            phone.Hided = phoneSpecFront.Hided;

            await _phonesRep.InsertOrUpdateAsync(phone, token);
            await BrandInsertIfNotExistAsync(phone.BrandSlug, token);
        }

        public async Task BrandInsertIfNotExistAsync(string brandSlug, CancellationToken token)
        {
            var brand = await _brandsRep.GetBySlugAsync(brandSlug, token);
            if (brand == null)
            {
                var brands = await _phoneSpecificationServiceApi.ListBrandsAsync(token);
                if (brands.Status == false)
                {
                    throw new Exception("PhoneSpecificationsApi not responds");
                }

                var brandDto = brands.Data.FirstOrDefault(b => b.Brand_slug == brandSlug);
                var brandE = _mapperProvider.GetMapper().Map<Brand>(brandDto);
                await _brandsRep.InsertAsync(brandE, token);
            }
        }
    }
}
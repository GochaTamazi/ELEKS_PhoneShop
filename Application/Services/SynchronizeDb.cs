using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services.RemoteAPI;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Models.DTO.RemoteAPI.ListBrands;
using Models.DTO.RemoteAPI.ListPhones;
using Models.DTO.RemoteAPI.PhoneSpecifications;

namespace Application.Services
{
    public class SynchronizeDb : ISynchronizeDb
    {
        private readonly IPhoneSpecificationClient _phoneSpecification;
        private readonly IBrands _rBrands;

        public SynchronizeDb(IPhoneSpecificationClient phoneSpecification, IBrands rBrands)
        {
            _phoneSpecification = phoneSpecification;
            _rBrands = rBrands;
        }

        public async Task BrandsAsync(CancellationToken token)
        {
            var listBrands = await _phoneSpecification.ListBrandsAsync(token);
            foreach (var brand in listBrands.Data)
            {
                var eBrand = new Models.Entities.RemoteApi.Brand()
                {
                    Name = brand.Brand_name,
                    Slug = brand.Brand_slug
                };
                await _rBrands.UpdateOrInsertAsync(eBrand, token);
            }
        }

        public async Task PhonesAsync(CancellationToken token)
        {
            ICollection<Brand> brands = new List<Brand>();
            foreach (var brand in brands)
            {
                GetPhonesAsync(brand.Brand_slug, 1, token);
            }
        }

        private async Task GetPhonesAsync(string brandSlug, int page, CancellationToken token)
        {
            var listPhones = await _phoneSpecification.ListPhonesAsync(brandSlug, page, token);
            var phones = listPhones.Data.Phones;
            Console.WriteLine($"BrandSlug: {brandSlug}; Page: {page}; Phones Count: {phones.Count}");
            if (listPhones.Data.Current_page < listPhones.Data.Last_page)
            {
                GetPhonesAsync(brandSlug, page + 1, token);
            }
        }

        public async Task SpecificationsAsync(CancellationToken token)
        {
            ICollection<Phone> phones = new List<Phone>();
            Console.WriteLine("SpecificationsAsync");
            Console.WriteLine($"Phones Count: {phones.Count}");

            var listPhoneDetail = new List<PhoneDetail>();
            foreach (var phone in phones)
            {
                var specifications = await _phoneSpecification.PhoneSpecificationsAsync(phone.Slug, token);
                listPhoneDetail.Add(specifications.Data);
            }
        }

        public async Task AllAsync(CancellationToken token)
        {
            await BrandsAsync(token);
            await PhonesAsync(token);
            await SpecificationsAsync(token);
        }
    }
}
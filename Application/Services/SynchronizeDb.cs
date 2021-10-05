using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using DataAccess.Interfaces;
using Models.Entities.RemoteApi;

namespace Application.Services
{
    public class SynchronizeDb : ISynchronizeDb
    {
        private readonly IPhoneSpecificationClient _phoneSpecification;

        private readonly IBrands _rBrands;
        private readonly IPhones _rPhones;

        public SynchronizeDb(IPhoneSpecificationClient phoneSpecification, IBrands rBrands, IPhones rPhones)
        {
            _phoneSpecification = phoneSpecification;
            _rBrands = rBrands;
            _rPhones = rPhones;
        }

        public async Task BrandsAsync(CancellationToken token)
        {
            var listBrands = await _phoneSpecification.ListBrandsAsync(token);
            foreach (var brand in listBrands.Data)
            {
                var eBrand = new Brand()
                {
                    Name = brand.Brand_name,
                    Slug = brand.Brand_slug
                };
                await _rBrands.UpdateOrInsertAsync(eBrand, token);
            }
        }

        public async Task PhonesAsync(CancellationToken token)
        {
            var brands = await _rBrands.ListAsync(token);
            foreach (var brand in brands)
            {
                GetPhonesAsync(brand, 1, token);
            }
        }

        private async Task GetPhonesAsync(Brand brand, int page, CancellationToken token)
        {
            var listPhones = await _phoneSpecification.ListPhonesAsync(brand.Slug, page, token);
            var phones = listPhones.Data.Phones;

            Console.WriteLine($"BrandSlug: {brand.Slug}; Page: {page}; Phones Count: {phones.Count}");

            foreach (var phone in phones)
            {
                var ePhone = new Phone()
                {
                    BrandId = brand.Id,
                    Name = phone.Phone_name,
                    Slug = phone.Slug,
                    Image = phone.Image
                };
                await _rPhones.UpdateOrInsertAsync(ePhone, token);
            }

            if (listPhones.Data.Current_page < listPhones.Data.Last_page)
            {
                GetPhonesAsync(brand, page + 1, token);
            }
        }

        public async Task SpecificationsAsync(CancellationToken token)
        {
            /*ICollection<Phone> phones = new List<Phone>();
            Console.WriteLine("SpecificationsAsync");
            Console.WriteLine($"Phones Count: {phones.Count}");

            var listPhoneDetail = new List<PhoneDetail>();
            foreach (var phone in phones)
            {
                var specifications = await _phoneSpecification.PhoneSpecificationsAsync(phone.Slug, token);
                listPhoneDetail.Add(specifications.Data);
            }*/
        }
    }
}
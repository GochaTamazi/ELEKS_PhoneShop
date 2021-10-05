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
        private IPhoneSpecificationClient PhoneSpecification { get; set; }
        private IBrands RBrands { get; set; }
        private IPhones RPhones { get; set; }

        public SynchronizeDb(IPhoneSpecificationClient phoneSpecification, IBrands rBrands, IPhones rPhones)
        {
            PhoneSpecification = phoneSpecification;
            RBrands = rBrands;
            RPhones = rPhones;
        }

        public async Task BrandsAsync(CancellationToken token)
        {
            var listBrands = await PhoneSpecification.ListBrandsAsync(token);
            foreach (var brand in listBrands.Data)
            {
                var eBrand = new Brand()
                {
                    Name = brand.Brand_name,
                    Slug = brand.Brand_slug
                };
                await RBrands.UpdateOrInsertAsync(eBrand, token);
            }
        }

        public async Task PhonesAsync(CancellationToken token)
        {
            var brands = await RBrands.ListAsync(token);
            foreach (var brand in brands)
            {
                GetPhonesAsync(brand, 1, token);
            }
        }

        private async Task GetPhonesAsync(Brand brand, int page, CancellationToken token)
        {
            var listPhones = await PhoneSpecification.ListPhonesAsync(brand.Slug, page, token);
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
                await RPhones.UpdateOrInsertAsync(ePhone, token);
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
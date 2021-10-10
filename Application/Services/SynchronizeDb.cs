using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using DataAccess.Interfaces;
using Models.Entities.RemoteApi;
using Newtonsoft.Json;

namespace Application.Services
{
    public class SynchronizeDb : ISynchronizeDb
    {
        private readonly IPhoneSpecificationClient _phoneSpecification;
        private readonly IBrands _rBrands;
        private readonly IPhones _rPhones;
        private readonly ISpecification _rSpecification;

        public SynchronizeDb(IPhoneSpecificationClient phoneSpecification, IBrands rBrands, IPhones rPhones,
            ISpecification rSpecification)
        {
            _phoneSpecification = phoneSpecification;
            _rBrands = rBrands;
            _rPhones = rPhones;
            _rSpecification = rSpecification;
        }

        public async Task BrandsAsync(CancellationToken token)
        {
            var listBrands = await _phoneSpecification.ListBrandsAsync(token);
            if (listBrands.Status)
            {
                var brands = listBrands.Data.Select(brand => new Brand()
                {
                    Name = brand.Brand_name,
                    Slug = brand.Brand_slug
                }).ToList();
                await _rBrands.BulkInsertOrUpdate(brands, token);
            }
        }

        public async Task PhonesAsync(CancellationToken token)
        {
            var brands = await _rBrands.ListAsync(token);
            var tasks = brands.Select(brand => GetPhonesAsync(brand, token)).ToList();
            var tasksResults = await Task.WhenAll(tasks);
            var allPhones = tasksResults.SelectMany(x => x).ToList();
            await _rPhones.BulkInsertOrUpdate(allPhones, token);
        }

        private async Task<List<Phone>> GetPhonesAsync(Brand brand, CancellationToken token)
        {
            var listPhones = await _phoneSpecification.ListPhonesAsync2(brand.Slug, 1, token);
            if (listPhones.Status == false)
            {
                return new List<Phone>();
            }

            Phone ConvertPhone(int brandId, Models.DTO.RemoteAPI.ListPhones.Phone phone)
            {
                return new Models.Entities.RemoteApi.Phone()
                {
                    BrandId = brandId,
                    Name = phone.Phone_name,
                    Slug = phone.Slug,
                    Image = phone.Image
                };
            }

            var phonesBatch = new ConcurrentBag<Phone>();
            foreach (var phone in listPhones.Data.Phones)
            {
                phonesBatch.Add(ConvertPhone(brand.Id, phone));
            }

            var tasks = new List<Task>();
            for (var page = 2; page <= listPhones.Data.Last_page; page++)
            {
                tasks.Add(_phoneSpecification.ListPhonesAsync2(brand.Slug, page, token)
                    .ContinueWith(x =>
                    {
                        if (x.Exception != null)
                        {
                            throw x.Exception;
                        }

                        foreach (var phone in x.Result.Data.Phones)
                        {
                            phonesBatch.Add(ConvertPhone(brand.Id, phone));
                        }
                    }, token));
            }

            await Task.WhenAll(tasks);
            return phonesBatch.ToList();
        }

        public async Task SpecificationsAsync(CancellationToken token)
        {
            var phones = await _rPhones.ListAsync(token);

            var tasks = new List<Task<Specification>>();
            var i = 0;
            foreach (var phone in phones)
            {
                tasks.Add(GetSpecificationsAsync(phone, token));
                i++;
                if (i > 10)
                {
                    break;
                }
            }

            //var tasks = phones.Select(phone => GetSpecificationsAsync(phone, token)).ToList();
            var tasksResults = await Task.WhenAll(tasks);
            var allSpecifications = tasksResults.ToList();
            Console.WriteLine(allSpecifications.Count);
            await _rSpecification.BulkInsertOrUpdate(allSpecifications, token);
            Console.WriteLine("DONE !!!");
        }

        private async Task<Specification> GetSpecificationsAsync(Phone phone, CancellationToken token)
        {
            Console.Write($"{phone.Id} ");
            var specification = await _phoneSpecification.PhoneSpecificationsAsync2(phone.Slug, token);
            if (specification.Status == false)
            {
                return new Specification();
            }

            var eSpecification = new Specification()
            {
                BrandId = phone.BrandId,
                PhoneId = phone.Id,
                Name = specification.Data.Phone_name,
                ReleaseDate = specification.Data.Release_date,
                Dimension = specification.Data.Dimension,
                Os = specification.Data.Os,
                Storage = specification.Data.Storage,
                Thumbnail = specification.Data.Thumbnail,
                Images = JsonConvert.SerializeObject(specification.Data.Phone_images, Formatting.Indented),
                Specifications = JsonConvert.SerializeObject(specification.Data.Specifications, Formatting.Indented)
            };
            return eSpecification;
        }
    }
}
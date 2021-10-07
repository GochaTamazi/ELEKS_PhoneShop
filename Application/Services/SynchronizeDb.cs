using System;
using System.Collections.Concurrent;
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
            if (listBrands.Status)
            {
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
        }

        public async Task PhonesAsync(CancellationToken token)
        {
            var brands = await _rBrands.ListAsync(token);
            var tasks = brands.Select(brand => GetPhonesAsync(brand, token)).ToList();
            var tasksResults = await Task.WhenAll(tasks);
            var allPhones = tasksResults.SelectMany(x => x).ToList();

            Console.WriteLine($"Phones cnt: {allPhones.Count}");
            var i = 0;
            foreach (var phone in allPhones)
            {
                await _rPhones.UpdateOrInsertAsync(phone, token);
                if (i % 100 == 0)
                {
                    Console.Write($"{i} ");
                }

                i++;
            }
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
                            Console.WriteLine(x.Exception);
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
        }
    }
}
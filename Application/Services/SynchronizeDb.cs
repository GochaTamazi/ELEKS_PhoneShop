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

        private readonly IMapperProvider _mapper;


        public SynchronizeDb(IPhoneSpecificationClient phoneSpecification, IBrands rBrands, IPhones rPhones,
            ISpecification rSpecification, IMapperProvider mapper)
        {
            _phoneSpecification = phoneSpecification;
            _rBrands = rBrands;
            _rPhones = rPhones;
            _rSpecification = rSpecification;
            _mapper = mapper;
        }

        public async Task BrandsAsync(CancellationToken token)
        {
            var listBrands = await _phoneSpecification.ListBrandsAsync(token);
            if (listBrands.Status)
            {
                var brands = listBrands.Data.Select(brand =>
                    _mapper.GetMapper().Map<Models.Entities.RemoteApi.Brand>(brand)
                ).ToList();
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

            var phonesBatch = new ConcurrentBag<Phone>();
            foreach (var phone in listPhones.Data.Phones)
            {
                var p = _mapper.GetMapper().Map<Models.Entities.RemoteApi.Phone>(phone);
                p.BrandId = brand.Id;
                phonesBatch.Add(p);
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
                            var p = _mapper.GetMapper().Map<Models.Entities.RemoteApi.Phone>(phone);
                            p.BrandId = brand.Id;
                            phonesBatch.Add(p);
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
                if (i > 15)
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

            var eSpecification = _mapper.GetMapper().Map<Models.Entities.RemoteApi.Specification>(specification);
            eSpecification.BrandId = phone.BrandId;
            eSpecification.PhoneId = phone.Id;
            return eSpecification;
        }
    }
}
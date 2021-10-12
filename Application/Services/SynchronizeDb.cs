using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.RemoteAPI;
using DataAccess.Interfaces;
using Models.Entities.RemoteApi;

namespace Application.Services
{
    public class SynchronizeDb : ISynchronizeDb
    {
        private readonly IPhoneSpecificationsApi _phoneSpecification;
        private readonly IBrandsRep _rBrandsRep;
        private readonly IPhonesRemoteApiRep _rPhonesRemoteApiRep;
        private readonly ISpecificationRep _rSpecificationRep;
        private readonly IMapperProvider _mapper;

        public SynchronizeDb(IPhoneSpecificationsApi phoneSpecification, IBrandsRep rBrandsRep, IPhonesRemoteApiRep rPhonesRemoteApiRep,
            ISpecificationRep rSpecificationRep, IMapperProvider mapper)
        {
            _phoneSpecification = phoneSpecification;
            _rBrandsRep = rBrandsRep;
            _rPhonesRemoteApiRep = rPhonesRemoteApiRep;
            _rSpecificationRep = rSpecificationRep;
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
                await _rBrandsRep.BulkInsertOrUpdate(brands, token);
            }
        }

        public async Task PhonesAsync(CancellationToken token)
        {
            var brands = await _rBrandsRep.ListAsync(token);
            var tasks = brands.Select(brand => GetPhonesAsync(brand, token)).ToList();
            var tasksResults = await Task.WhenAll(tasks);
            var allPhones = tasksResults.SelectMany(x => x).ToList();
            await _rPhonesRemoteApiRep.BulkInsertOrUpdate(allPhones, token);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public async Task SpecificationsAsync(CancellationToken token)
        {
            var phones = await _rPhonesRemoteApiRep.ListAsync(token);

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
            await _rSpecificationRep.BulkInsertOrUpdate(allSpecifications, token);
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
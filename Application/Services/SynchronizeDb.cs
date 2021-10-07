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
            //SyncPhonesCounter = brands.Count();
            _syncPhonesCounter = 9;
            int i = 0;
            foreach (var brand in brands)
            {
                i++;
                if (i >= 10)
                {
                    break;
                }

                await GetPhonesAsync(brand, 1, token);
            }
        }

        private int _syncPhonesCounter = 0;
        private ConcurrentQueue<Phone> _phonesQueue = new ConcurrentQueue<Phone>();
        private int _totalCountPhones = 0;

        private async Task GetPhonesAsync(Brand brand, int page, CancellationToken token)
        {
            var listPhones = await _phoneSpecification.ListPhonesAsync(brand.Slug, page, token);
            if (listPhones.Status)
            {
                var phones = listPhones.Data.Phones;
                foreach (var phone in phones)
                {
                    var ePhone = new Phone()
                    {
                        BrandId = brand.Id,
                        Name = phone.Phone_name,
                        Slug = phone.Slug,
                        Image = phone.Image
                    };
                    _phonesQueue.Enqueue(ePhone);
                    _totalCountPhones++;
                }

                Console.WriteLine(
                    $"Slug: {brand.Slug}; Page: {listPhones.Data.Current_page}/{listPhones.Data.Last_page}; " +
                    $"PCount: {phones.Count}; TPC: {_totalCountPhones}; BCNT: {_syncPhonesCounter}"
                );
            }

            if (listPhones.Data.Current_page < listPhones.Data.Last_page)
            {
                //GetPhonesAsync(brand, page + 1, token);
            }

            //else
            {
                _syncPhonesCounter--;
                if (_syncPhonesCounter <= 0)
                {
                    var total = _phonesQueue.Count;
                    Console.WriteLine($"PQ: {total}; ");
                    var i = 0;

                    while (_phonesQueue.Count > 0)
                    {
                        if (_phonesQueue.TryDequeue(out var phone))
                        {
                            i++;
                            Console.Write($"{i}/{total} ");
                            await _rPhones.UpdateOrInsertAsync(phone, token);
                        }
                    }

                    Console.WriteLine("Done:");
                    _phonesQueue.Clear();
                }
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
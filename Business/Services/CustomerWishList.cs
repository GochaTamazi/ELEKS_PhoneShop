using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using DataAccess.Interfaces;
using Database.Models;

namespace Application.Services
{
    public class CustomerWishList : ICustomerWishList
    {
        private readonly IGeneralRepository<Phone> _phonesRepository;
        private readonly IGeneralRepository<User> _usersRepository;
        private readonly IGeneralRepository<WishList> _wishListRepository;

        public CustomerWishList(
            IGeneralRepository<Phone> phonesRepository,
            IGeneralRepository<User> usersRepository,
            IGeneralRepository<WishList> wishListRepository
        )
        {
            _phonesRepository = phonesRepository;
            _usersRepository = usersRepository;
            _wishListRepository = wishListRepository;
        }

        public async Task InsertIfNotExistAsync(string phoneSlug, string userMail, CancellationToken token)
        {
            var phone = await _phonesRepository.GetOneAsync(p => p.PhoneSlug == phoneSlug && p.Hided != true, token);
            var user = await _usersRepository.GetOneAsync(user => user.Email == userMail, token);
            if (phone != null && user != null)
            {
                var wishList = new WishList()
                {
                    UserId = user.Id,
                    PhoneId = phone.Id
                };

                await _wishListRepository.InsertIfNotExistAsync(l => l.UserId == user.Id && l.PhoneId == phone.Id,
                    wishList, token);
            }
        }

        public async Task DeleteAsync(string phoneSlug, string userMail, CancellationToken token)
        {
            var phone = await _phonesRepository.GetOneAsync(p => p.PhoneSlug == phoneSlug && p.Hided != true, token);
            var user = await _usersRepository.GetOneAsync(user => user.Email == userMail, token);
            if (phone != null && user != null)
            {
                await _wishListRepository.RemoveIfExistAsync(l => l.UserId == user.Id && l.PhoneId == phone.Id, token);
            }
        }

        public async Task<List<WishList>> GetAllAsync(string userMail, CancellationToken token)
        {
            var user = await _usersRepository.GetOneAsync(user => user.Email == userMail, token);
            if (user != null)
            {
                return await _wishListRepository.GetAllIncludeAsync(l => l.UserId == user.Id, l => l.Phone, token);
            }

            return new List<WishList>();
        }
    }
}
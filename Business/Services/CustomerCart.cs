using Application.Interfaces;
using DataAccess.Interfaces;
using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Services
{
    public class CustomerCart : ICustomerCart
    {
        private readonly IGeneralRepository<User> _usersRepository;
        private readonly IGeneralRepository<Phone> _phonesRepository;
        private readonly IGeneralRepository<Cart> _cartsRepository;

        public CustomerCart(
            IGeneralRepository<User> usersRepository,
            IGeneralRepository<Phone> phonesRepository,
            IGeneralRepository<Cart> cartsRepository
        )
        {
            _usersRepository = usersRepository;
            _phonesRepository = phonesRepository;
            _cartsRepository = cartsRepository;
        }

        public async Task AddOrUpdateAsync(string phoneSlug, string userMail, int amount, CancellationToken token)
        {
            var phone = await _phonesRepository.GetOneAsync(p =>
                    p.PhoneSlug == phoneSlug &&
                    p.Stock - amount > 0 &&
                    p.Hided != true,
                token);
            var user = await _usersRepository.GetOneAsync(user => user.Email == userMail, token);
            if (phone != null && user != null)
            {
                var cart = new Cart()
                {
                    UserId = user.Id,
                    PhoneId = phone.Id,
                    Amount = amount
                };
                await _cartsRepository.AddOrUpdateAsync(c => c.UserId == user.Id && c.PhoneId == phone.Id,
                    cart,
                    token);
            }
        }

        public async Task RemoveAsync(string phoneSlug, string userMail, CancellationToken token)
        {
            var phone = await _phonesRepository.GetOneAsync(p => p.PhoneSlug == phoneSlug && p.Hided != true, token);
            var user = await _usersRepository.GetOneAsync(user => user.Email == userMail, token);
            if (phone != null && user != null)
            {
                await _cartsRepository.RemoveIfExistAsync(c => c.UserId == user.Id && c.PhoneId == phone.Id, token);
            }
        }

        public async Task<List<Cart>> BuyAsync(string userMail, CancellationToken token)
        {
            var carts = await GetAllAsync(userMail, token);

            var delCarts = new List<Cart>();
            if (carts != null)
            {
                foreach (var c in carts)
                {
                    var phone = c.Phone;
                    if (phone.Stock - c.Amount > 0)
                    {
                        phone.Stock -= c.Amount;
                        await _phonesRepository.UpdateAsync(phone, token);
                        var delCart = await _cartsRepository.RemoveAsync(c, token);
                        delCarts.Add(delCart);
                    }
                }
            }

            return delCarts;
        }

        public async Task<List<Cart>> GetAllAsync(string userMail, CancellationToken token)
        {
            var user = await _usersRepository.GetOneAsync(user => user.Email == userMail, token);
            if (user != null)
            {
                return await _cartsRepository.GetAllIncludeAsync(c => c.UserId == user.Id, c => c.Phone, token);
            }

            return null;
        }
    }
}
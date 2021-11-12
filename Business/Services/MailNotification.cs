using Application.Interfaces;
using Database.Models;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading;
using System;
using DataAccess.Interfaces;

namespace Application.Services
{
    public class MailNotification : IMailNotification
    {
        private readonly IEmail _mail;
        private readonly IGeneralRepository<PriceSubscriber> _priceSubscribersRepository;
        private readonly IGeneralRepository<StockSubscriber> _stockSubscribersRepository;
        private readonly IGeneralRepository<WishList> _wishListRepository;

        public MailNotification(
            IEmail mail,
            IGeneralRepository<PriceSubscriber> priceSubscribersRepository,
            IGeneralRepository<StockSubscriber> stockSubscribersRepository,
            IGeneralRepository<WishList> wishListRepository
        )
        {
            _mail = mail;
            _priceSubscribersRepository = priceSubscribersRepository;
            _stockSubscribersRepository = stockSubscribersRepository;
            _wishListRepository = wishListRepository;
        }

        public async Task StockSubscribersNotificationAsync(Phone phone, CancellationToken token)
        {
            var subscribers = await _stockSubscribersRepository.GetAllAsync(s =>
                s.BrandSlug == phone.BrandSlug && s.PhoneSlug == phone.PhoneSlug, token);

            var subject = $"The phone {phone.PhoneName} is back in stock";
            var url = $"http://localhost:5000/customer/showPhone?phoneSlug={phone.PhoneSlug}";
            var body = $@"
                <b>The phone {phone.PhoneName} is back in stock</b> <br/>
                <b>Current on stock:</b> {phone.Stock}<br/>
                <b><a href='{url}'>Check it out!</a></b>
            ";

            var tasks = subscribers.Select(sub => SendMailAsync(sub.Email, subject, body, token)).ToList();
            await Task.WhenAll(tasks);
        }

        public async Task PriceWishListCustomerNotificationAsync(Phone phone, CancellationToken token)
        {
            var subscribers = await _wishListRepository.GetAllIncludeAsync(l => l.PhoneId == phone.Id,
                l => l.User, token);

            var subject = $"WishList Phone {phone.PhoneName} price has changed";
            var url = $"http://localhost:5000/customer/showPhone?phoneSlug={phone.PhoneSlug}";
            var body = $@"
                <b>Phone {phone.PhoneName} price has changed</b> <br/>
                <b>New price:</b> {phone.Price}<br/>
                <b><a href='{url}'>Check it out!</a></b> 
            ";

            var tasks = subscribers.Select(sub => SendMailAsync(sub.User.Email, subject, body, token)).ToList();
            await Task.WhenAll(tasks);
        }

        public async Task PriceSubscribersNotificationAsync(Phone phone, CancellationToken token)
        {
            var subscribers = await _priceSubscribersRepository.GetAllAsync(s =>
                s.BrandSlug == phone.BrandSlug && s.PhoneSlug == phone.PhoneSlug, token);

            var subject = $"PriceSubscribers Phone {phone.PhoneName} price has changed";
            var url = $"http://localhost:5000/customer/showPhone?phoneSlug={phone.PhoneSlug}";
            var body = $@"
                <b>Phone {phone.PhoneName} price has changed</b> <br/>
                <b>New price:</b> {phone.Price}<br/>
                <b><a href='{url}'>Check it out!</a></b> 
            ";

            var tasks = subscribers.Select(sub => SendMailAsync(sub.Email, subject, body, token)).ToList();
            await Task.WhenAll(tasks);
        }

        private async Task SendMailAsync(string email, string subject, string body, CancellationToken token)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                await _mail.SendEmailAsync(mailAddress, subject, body, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
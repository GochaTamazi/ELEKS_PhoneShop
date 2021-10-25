using Application.Interfaces;
using Database.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Application.Services
{
    public class MailNotification : IMailNotification
    {
        private readonly IEmail _mail;

        public MailNotification(IEmail mail)
        {
            _mail = mail;
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

        public async Task PriceSubscribersNotificationAsync(List<PriceSubscriber> subscribers, Phone phone,
            CancellationToken token)
        {
            var subject = $"Phone {phone.PhoneName} price has changed";

            var url = $"http://localhost:5000/customer/showPhone?phoneSlug={phone.PhoneSlug}";
            var body = $@"
                <b>Phone {phone.PhoneName} price has changed</b> <br/>
                <b>New price:</b> {phone.Price}<br/>
                <b><a href='{url}'>Check it out!</a></b> 
            ";

            var tasks = subscribers
                .Select(sub => SendMailAsync(sub.Email, subject, body, token))
                .ToList();

            await Task.WhenAll(tasks);
        }

        public async Task StockSubscribersNotificationAsync(List<StockSubscriber> subscribers, Phone phone,
            CancellationToken token)
        {
            var subject = $"The phone {phone.PhoneName} is back in stock";

            var url = $"http://localhost:5000/customer/showPhone?phoneSlug={phone.PhoneSlug}";
            var body = $@"
                <b>The phone {phone.PhoneName} is back in stock</b> <br/>
                <b>Current on stock:</b> {phone.Stock}<br/>
                <b><a href='{url}'>Check it out!</a></b>
            ";

            var tasks = subscribers
                .Select(sub => SendMailAsync(sub.Email, subject, body, token))
                .ToList();

            await Task.WhenAll(tasks);
        }
    }
}
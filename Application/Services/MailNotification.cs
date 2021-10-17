using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Models.Entities;
using System.Net.Mail;

namespace Application.Services
{
    public class MailNotification : IMailNotification
    {
        private readonly IEmail _mail;

        public MailNotification(IEmail mail)
        {
            _mail = mail;
        }

        private async Task SendMailAsync(string email, string subject, string body,
            CancellationToken token)
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

        public async Task PriceSubscribersNotificationAsync(List<PriceSubscriber> subs, Phone phone,
            CancellationToken token)
        {
            var subject = $"Phone {phone.PhoneName} price has changed";

            var url = $"http://localhost:5000/customer/showPhone?phoneSlug={phone.PhoneSlug}";
            var body = $@"
                <b>Phone {phone.PhoneName} price has changed</b> <br/>
                <b>New price:</b> {phone.Price}<br/>
                <b><a href='{url}'>Check it out!</a></b> 
            ";

            var tasks = subs.Select(sub => SendMailAsync(sub.Email, subject, body, token)).ToList();
            await Task.WhenAll(tasks);
        }

        public async Task StockSubscribersNotificationAsync(List<StockSubscriber> subs, Phone phone,
            CancellationToken token)
        {
            var subject = $"The phone {phone.PhoneName} is back in stock";

            var url = $"http://localhost:5000/customer/showPhone?phoneSlug={phone.PhoneSlug}";
            var body = $@"
                <b>The phone {phone.PhoneName} is back in stock</b> <br/>
                <b>Current on stock:</b> {phone.Price}<br/>
                <b><a href='{url}'>Check it out!</a></b>
            ";

            var tasks = subs.Select(sub => SendMailAsync(sub.Email, subject, body, token)).ToList();
            await Task.WhenAll(tasks);
        }
    }
}
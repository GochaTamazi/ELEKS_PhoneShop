using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Options;
using Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class Email : IEmail
    {
        private readonly EmailOptions _emailOptions;

        public Email(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public async Task SendEmailAsync(MailAddress to, string subject, string html, CancellationToken token)
        {
            Console.WriteLine("Email.SendMailAsync");

            Console.WriteLine($"SmtpHost: {_emailOptions.SmtpHost}");
            Console.WriteLine($"SmtpPort: {_emailOptions.SmtpPort}");
            Console.WriteLine($"SmtpUser: {_emailOptions.SmtpUser}");
            Console.WriteLine($"SmtpPass: {_emailOptions.SmtpPass}");

            Console.WriteLine($"MailAddress: {to}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {html}");

            var from = new MailAddress(_emailOptions.SmtpUser);
            var message = new MailMessage(from, to)
            {
                Subject = subject, Body = html, IsBodyHtml = true
            };

            var smptClient = new SmtpClient(_emailOptions.SmtpHost, Convert.ToInt32(_emailOptions.SmtpPort))
            {
                Credentials = new NetworkCredential(_emailOptions.SmtpUser, _emailOptions.SmtpPass), EnableSsl = true
            };

            await smptClient.SendMailAsync(message, token);
        }
    }
}
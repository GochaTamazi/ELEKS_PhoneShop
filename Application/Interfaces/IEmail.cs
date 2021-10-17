using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmail
    {
        public Task SendEmailAsync(MailAddress to, string subject, string html, CancellationToken token);
    }
}
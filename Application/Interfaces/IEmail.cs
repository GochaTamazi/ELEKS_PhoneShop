using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface IEmail
    {
        public Task SendEmailAsync(MailAddress to, string subject, string html, CancellationToken token);
    }
}
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmail
    {
        public Task SendEmail(MailAddress to, string subject, string html);
    }
}
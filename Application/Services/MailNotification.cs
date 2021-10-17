using Application.Interfaces;

namespace Application.Services
{
    public class MailNotification : IMailNotification
    {
        private IEmail _mail;

        public MailNotification(IEmail mail)
        {
            _mail = mail;
        }
        
    }
}   
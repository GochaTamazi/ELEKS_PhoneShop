using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.RemoteAPI;

namespace Application.Services
{
    public class AdminPhones : IAdminPhones
    {
        private readonly IPhoneSpecificationsApi _phoneSpecification;

        public AdminPhones(IPhoneSpecificationsApi phoneSpecification)
        {
            _phoneSpecification = phoneSpecification;
        }

        public async Task GetPhoneSpecificationsAsync(string phoneSlug, CancellationToken token)
        {
            
            
        }
    }
}
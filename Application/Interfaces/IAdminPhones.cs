using System.Threading;
using System.Threading.Tasks;
using Models.Entities.PhoneShop;

namespace Application.Interfaces
{
    public interface IAdminPhones
    {
        Task GetPhoneSpecificationsAsync(string phoneSlug, CancellationToken token);
        
        /*void GetPhone(string phoneSlug);
        void AddPhone(string phoneSlug, Phone p );
        void UpdatePhone(string phoneSlug, Phone p);*/
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;
using Application.Interfaces;

namespace Application.Services
{
    public class CustomerCart : ICustomerCart
    {
        public Task InsertAsync(string phoneSlug, string userMail, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(string phoneSlug, string userMail, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task BuyAsync(string userMail, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task UsePromoCodeAsync(string code, string userMail, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<PhoneDto>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
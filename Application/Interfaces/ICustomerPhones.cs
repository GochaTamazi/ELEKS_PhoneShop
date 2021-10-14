using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.Frontend;
using Models.Entities;

namespace Application.Interfaces
{
    public interface ICustomerPhones
    {
        Task<List<Phone>> GetPhonesAsync(PhonesFilter filter, CancellationToken token);
        Task<PhoneDto> GetPhoneAsync(string phoneSlug, CancellationToken token);
    }
}
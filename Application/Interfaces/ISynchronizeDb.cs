using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.DTO.RemoteAPI.ListBrands;
using Models.DTO.RemoteAPI.ListPhones;
using Models.DTO.RemoteAPI.PhoneSpecifications;

namespace Application.Interfaces
{
    public interface ISynchronizeDb
    {
        Task BrandsAsync(CancellationToken token);
        Task PhonesAsync(CancellationToken token);
        Task SpecificationsAsync(CancellationToken token);
        Task AllAsync(CancellationToken token);
    }
}
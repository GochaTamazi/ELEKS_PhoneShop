using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace Application.Interfaces
{
    public interface IPhoneData
    {
        Task<byte[]> ExportToXlsxAsync(List<Phone> phones, CancellationToken token);
    }
}
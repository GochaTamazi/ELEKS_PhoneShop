using Database.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Interfaces
{
    public interface IPhoneData
    {
        Task<byte[]> ExportToXlsxAsync(List<Phone> phones, CancellationToken token);

        Task<List<Phone>> ImportFromXlsxAsync(Stream stream, CancellationToken token);
    }
}
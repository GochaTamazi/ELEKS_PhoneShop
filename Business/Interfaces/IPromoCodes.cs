using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;

namespace Application.Interfaces
{
    public interface IPromoCodes
    {
        Task<List<PromoCode>> GetAllAsync(CancellationToken token);
    }
}
using System.Threading;
using System.Threading.Tasks;
using Models.Entities;

namespace DataAccess.Interfaces
{
    public interface IBrandsRepository
    {
        Task<Brand> GetByIdAsync(int id, CancellationToken token);
        Task<Brand> GetByNameAsync(string name, CancellationToken token);
        Task<Brand> GetBySlugAsync(string slug, CancellationToken token);
        Task InsertAsync(Brand brand, CancellationToken token);
    }
}
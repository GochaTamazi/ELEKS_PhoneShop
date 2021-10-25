using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace DataAccess.Repositories
{
    public class BrandsRepository : IBrandsRepository
    {
        private readonly MasterContext _masterContext;

        public BrandsRepository(MasterContext dbContext)
        {
            _masterContext = dbContext;
        }

        public async Task<Brand> GetByIdAsync(int id, CancellationToken token)
        {
            return await _masterContext.Brands.Where(brand => brand.Id == id).FirstOrDefaultAsync(token);
        }

        public async Task<Brand> GetByNameAsync(string name, CancellationToken token)
        {
            return await _masterContext.Brands.Where(brand => brand.Name == name).FirstOrDefaultAsync(token);
        }

        public async Task<Brand> GetBySlugAsync(string slug, CancellationToken token)
        {
            return await _masterContext.Brands.Where(brand => brand.Slug == slug).FirstOrDefaultAsync(token);
        }

        public async Task InsertAsync(Brand brand, CancellationToken token)
        {
            await _masterContext.Brands.AddAsync(brand, token);
            await _masterContext.SaveChangesAsync(token);
        }
    }
}
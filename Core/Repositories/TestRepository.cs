using System;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Core.Repositories
{
    public class TestRepository
    {
        private readonly MasterContext _masterContext;

        public TestRepository(MasterContext masterContext)
        {
            this._masterContext = masterContext;
        }

        public async Task GetAll(CancellationToken token)
        {
            var tests = await _masterContext.Tests.ToListAsync(token);
            foreach (var v in tests)
            {
                Console.WriteLine($"{v.Id} {v.Name}");
            }
        }
    }
}
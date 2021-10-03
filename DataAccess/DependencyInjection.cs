using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Models.Entities;

namespace DataAccess
{
    public static class DependencyInjection
    {
        public static void Configure(IServiceCollection services)
        {
            //services.AddScoped<IGeneric<Test>, Generic<Test>>();
        }
    }
}
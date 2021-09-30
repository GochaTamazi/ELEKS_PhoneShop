using Core.Interfaces;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class DiConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IPhoneSpecificationClient, PhoneSpecificationClient>();
            services.AddScoped<TestRepository>();
        }
    }
}
using Application.Interfaces;
using Application.Services;
using Application.Services.RemoteAPI;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IPhoneSpecificationClient, PhoneSpecificationClient>();
            services.AddScoped<ITestService, TestService>();
        }
    }
}
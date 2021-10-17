using System;
using Application.DTO.Options;
using Application.Interfaces;
using Application.Services;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PhoneShop
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            //Database Context
            services.AddDbContext<MasterContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            //DataAccess Repositories
            services.AddScoped<IBrandsRep, BrandsRep>();
            services.AddScoped<IPhonesRep, PhonesRep>();
            services.AddScoped<IPriceSubscribersRep, PriceSubscribersRep>();
            services.AddScoped<IStockSubscribersRep, StockSubscribersRep>();

            //Application Services
            services.AddScoped<IPhoneSpecificationsApi, PhoneSpecificationsApi>();
            services.AddScoped<IAdminPhones, AdminPhones>();
            services.AddScoped<ICustomerPhones, CustomerPhones>();

            services.AddSingleton<IMapperProvider, MapperProvider>();
            services.AddSingleton(serviceProvider =>
            {
                var provider = serviceProvider.GetRequiredService<IMapperProvider>();
                return provider.GetMapper();
            });

            //Email Service
            var configurationSectionEmailService = Configuration.GetSection("Email");
            services.Configure<EmailOptions>(configurationSectionEmailService);
            services.AddSingleton<IEmail, Email>();
            services.AddSingleton<IMailNotification, MailNotification>();

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            services.AddControllers();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
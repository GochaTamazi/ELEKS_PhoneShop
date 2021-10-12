using Application.Interfaces;
using Application.Interfaces.RemoteAPI;
using Application.Services;
using Application.Services.RemoteAPI;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Entities.PhoneShop;
using Models.Entities.RemoteApi;

namespace PhoneShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Database Context
            services.AddDbContext<MasterContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            //DataAccess Repositories
            services.AddScoped<IGenericRep<Brand>, GenericRep<Brand>>();
            services.AddScoped<IGenericRep<Specification>, GenericRep<Specification>>();
            services.AddScoped<IGenericRep<PriceSubscriber>, GenericRep<PriceSubscriber>>();
            services.AddScoped<IGenericRep<StockSubscriber>, GenericRep<StockSubscriber>>();
            services
                .AddScoped<IGenericRep<Models.Entities.RemoteApi.Phone>, GenericRep<Models.Entities.RemoteApi.Phone>>();
            services
                .AddScoped<IGenericRep<Models.Entities.PhoneShop.Phone>, GenericRep<Models.Entities.PhoneShop.Phone>>();

            services.AddScoped<IBrandsRep, BrandsRep>();
            services.AddScoped<ISpecificationRep, SpecificationRep>();
            services.AddScoped<IPriceSubscribersRep, PriceSubscribersRep>();
            services.AddScoped<IStockSubscribersRep, StockSubscribersRep>();
            services.AddScoped<IPhonesRemoteApiRep, PhonesRemoteApiRep>();
            services.AddScoped<IPhonesPhoneShopRep, PhonesPhoneShopRep>();

            //Application Services
            services.AddScoped<IPhoneSpecificationsApi, PhoneSpecificationsApi>();
            services.AddScoped<ISynchronizeDb, SynchronizeDb>();
            services.AddScoped<IAdminPhones, AdminPhones>();

            services.AddSingleton<IMapperProvider, MapperProvider>();
            services.AddSingleton(serviceProvider =>
            {
                var provider = serviceProvider.GetRequiredService<IMapperProvider>();
                return provider.GetMapper();
            });


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
using Application.DTO.Options;
using Application.Interfaces;
using Application.Services;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Database.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
            // Options
            var configurationPhoneSpecificationsApi = Configuration.GetSection("PhoneSpecificationsApi");
            services.Configure<PhoneSpecificationsApiOptions>(configurationPhoneSpecificationsApi);

            var configurationSectionEmailService = Configuration.GetSection("Email");
            services.Configure<EmailOptions>(configurationSectionEmailService);

            // Database Context
            services.AddDbContext<MasterContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            // DataAccess Repositories
            services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));

            // Business Services
            services.AddScoped<IAdminPhones, AdminPhones>();
            services.AddScoped<ICustomerCart, CustomerCart>();
            services.AddScoped<ICustomerComments, CustomerComments>();
            services.AddScoped<ICustomerPhones, CustomerPhones>();
            services.AddScoped<ICustomerWishList, CustomerWishList>();
            services.AddScoped<IEmail, Email>();
            services.AddScoped<IMailNotification, MailNotification>();
            services.AddScoped<IPhoneSpecificationsApi, PhoneSpecificationsApi>();
            services.AddScoped<IPromoCodes, PromoCodes>();
            services.AddScoped<ISubscribers, Subscribers>();

            // Mapper
            services.AddSingleton<IMapperProvider, MapperProvider>();
            services.AddSingleton(serviceProvider =>
            {
                var provider = serviceProvider.GetRequiredService<IMapperProvider>();
                return provider.GetMapper();
            });

            // Security
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/account/login");
                });

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
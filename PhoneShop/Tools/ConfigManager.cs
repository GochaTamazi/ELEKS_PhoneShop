using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace PhoneShop.Tools
{
    static class ConfigManager
    {
        public static IConfiguration AppSetting { get; set; }

        static ConfigManager()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
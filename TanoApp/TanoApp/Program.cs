using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TanoApp.Data.EF.EF;

namespace TanoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            using(var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                try
                {
                    var dbInitializer = service.GetService<DbInitializer>();
                    dbInitializer.Seed().Wait();
                } catch(Exception ex)
                {
                    var _logger = service.GetService<ILogger<Program>>();
                    _logger.LogError(ex, "Can't seeding data");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

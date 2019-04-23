using System.IO;
using System.Threading.Tasks;
using articledata.cardinformation.Services;
using cardprocessor;
using cardprocessor.application;
using cardprocessor.application.Configuration;
using cardprocessor.infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace carddata
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging((hostContext, config) =>
                {
                    config.AddConsole();
                    config.AddDebug();
                })
                .ConfigureHostConfiguration(config =>
                {
#if DEBUG
                    config.AddEnvironmentVariables(prefix: "ASPNETCORE_");
#else
                        config.AddEnvironmentVariables();
#endif
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddCommandLine(args);

                    #if DEBUG
                        config.AddUserSecrets<Program>();
                    #endif
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();

                    //configuration settings
                    services.Configure<RabbitMqSettings>(hostContext.Configuration.GetSection(nameof(RabbitMqSettings)));
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));

                    // hosted service
                    services.AddHostedService<CardProcessorHostedService>();

                    services.AddApplicationServices();
                    services.AddInfrastructureServices(hostContext.Configuration.GetConnectionString(DbConstants.YgoDatabase));
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                // Start the host
                await host.StartAsync();
            }

        }
    }
}

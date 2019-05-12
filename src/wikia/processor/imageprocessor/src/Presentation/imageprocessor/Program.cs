using System.IO;
using System.Threading.Tasks;
using articledata.cardinformation.Services;
using imageprocessor.application;
using imageprocessor.application.Configuration;
using imageprocessor.infrastructure;
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

                    // hosted service
                    services.AddHostedService<ImageProcessorHostedService>();

                    services.AddApplicationServices();
                    services.AddInfrastructureServices();
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

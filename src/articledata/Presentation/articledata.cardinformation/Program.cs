using articledata.application;
using articledata.application.Configuration;
using articledata.cardinformation.QuartzConfiguration;
using articledata.cardinformation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System.IO;
using System.Threading.Tasks;

namespace articledata.cardinformation
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

                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>();
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();

                    services.AddScoped<IJob, CardInformationJob>();
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));

                    // hosted service
                    services.AddHostedService<CardInformationHostedService>();

                    services.AddApplicationServices();
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                // Start the host
                await host.StartAsync();

                // Wait for the host to shutdown
                await host.WaitForShutdownAsync();
            }

        }
    }
}

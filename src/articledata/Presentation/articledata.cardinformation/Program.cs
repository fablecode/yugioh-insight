using System.Configuration;
using System.IO;
using articledata.application;
using articledata.cardinformation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;
using articledata.cardinformation.QuartzConfiguration;

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
                    config.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddCommandLine(args);

                    if (hostContext.HostingEnvironment.IsEnvironment("Development"))
                    {
                        // code to be executed in development environment 

                    }
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        config.AddUserSecrets<AppSettings>();
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                    }

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

    public class AppSettings
    {
        public string CronSchedule { get; set; }
    }
}

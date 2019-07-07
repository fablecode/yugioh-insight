using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using banlistprocessor.application;
using banlistprocessor.application.Configuration;
using banlistprocessor.Extensions.WindowsService;
using banlistprocessor.Services;
using cardprocessor.infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace banlistprocessor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            var hostBuilder = CreateHostBuilder(args);

            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            if (isService)
                await hostBuilder.RunAsServiceAsync();
            else
                await hostBuilder.RunConsoleAsync();
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs ex)
        {
            Log.Logger.Error("Unhandled exception occurred. Exception: {@Exception}", ex);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new HostBuilder()
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
                    services.AddHostedService<BanlistProcessorHostedService>();

                    services.AddApplicationServices();
                    services.AddInfrastructureServices(hostContext.Configuration.GetConnectionString(DbConstants.YgoDatabase));
                })
                .UseConsoleLifetime()
                .UseSerilog();
        }
    }
}

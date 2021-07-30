using article.application;
using article.application.Configuration;
using article.archetypes.cards.QuartzConfiguration;
using article.archetypes.cards.Services;
using article.infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace article.archetypes.cards
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

        #region private helpers

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder()
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

                    services.AddScoped<IJob, ArchetypeInformationJob>();

                    //configuration settings
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));
                    services.Configure<RabbitMqSettings>(hostContext.Configuration.GetSection(nameof(RabbitMqSettings)));

                    // hosted service
                    services.AddHostedService<ArchetypeWorkerService>();

                    services.AddApplicationServices();
                    services.AddInfrastructureServices();
                })
                .UseConsoleLifetime()
                .UseSerilog();

            return hostBuilder;
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs ex)
        {
            Log.Logger.Error("Unhandled exception occurred. Exception: {@Exception}", ex);
        }

        #endregion
    }
}

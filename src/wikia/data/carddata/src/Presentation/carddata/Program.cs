using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using articledata.cardinformation.Services;
using carddata.application;
using carddata.application.Configuration;
using carddata.Extensions.WindowsService;
using carddata.infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace carddata
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args);
        }

        private static async Task<IHostBuilder> CreateHostBuilder(string[] args)
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

                    //configuration settings
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));
                    services.Configure<RabbitMqSettings>(hostContext.Configuration.GetSection(nameof(RabbitMqSettings)));

                    var appSettings = services.BuildServiceProvider().GetService<IOptions<AppSettings>>();

                    // Create the logger
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.File(new JsonFormatter(renderMessage: true),
                            (appSettings.Value.LogFolder + $@"/carddata.{Environment.MachineName}.txt"),
                            fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                            rollingInterval: RollingInterval.Day)
                        .CreateLogger();

                    AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

                    // hosted service
                    services.AddHostedService<CardDataHostedService>();

                    services.AddApplicationServices();
                    services.AddInfrastructureServices();
                })
                .UseConsoleLifetime()
                .UseSerilog();

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

            return hostBuilder;
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Logger.Error("Unhandled exception occurred. Exception: {@Exception}", e);
        }

    }
}

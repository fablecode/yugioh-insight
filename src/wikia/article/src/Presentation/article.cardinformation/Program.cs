using System;
using System.Configuration;
using article.application;
using article.application.Configuration;
using article.cardinformation.QuartzConfiguration;
using article.cardinformation.Services;
using article.infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace article.cardinformation
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

                    services.AddScoped<IJob, CardInformationJob>();

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
                        .WriteTo.File(new JsonFormatter(renderMessage: true), (appSettings.Value.LogFolder + $@"/cardinformation.{Environment.MachineName}.txt"), fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day)
                        .CreateLogger();

                    AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

                    // hosted service
                    services.AddHostedService<CardInformationWorkerService>();

                    services.AddApplicationServices();
                    services.AddInfrastructureServices();
                })
                .UseConsoleLifetime()
                .UseSerilog()
                .Build();

            using (host)
            {
                // Start the host
                await host.StartAsync();

                // Wait for the host to shutdown
                await host.WaitForShutdownAsync();
            }

        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Logger.Error("Unhandled exception occurred. Exception: {@Exception}", e);
        }
    }
}

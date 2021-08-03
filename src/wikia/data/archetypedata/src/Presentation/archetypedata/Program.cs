using archetypedata.application;
using archetypedata.application.Configuration;
using archetypedata.infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using archetypedata.Services;

namespace archetypedata
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();
                }

                Log.Fatal(ex, "Host terminated unexpectedly");

                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();

                    //configuration settings
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));
                    services.Configure<RabbitMqSettings>(hostContext.Configuration.GetSection(nameof(RabbitMqSettings)));

                    // hosted service
                    //services.AddHostedService<ArchetypeDataHostedService>();
                    services.AddHostedService<ArchetypeCardDataHostedService>();
                    //services.AddHostedService<ArchetypeSupportCardDataHostedService>();

                    services.AddHttpClient();

                    services.AddApplicationServices();
                    services.AddInfrastructureServices();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);
                });
    }
}

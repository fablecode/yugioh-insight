﻿using carddata.application;
using carddata.application.Configuration;
using carddata.infrastructure;
using carddata.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace carddata
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
                    //services.AddHostedService<CardDataHostedService>();
                    services.AddHostedService<SemanticCardDataHostedService>();

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



    //    internal class Program
    //    {
    //        static async Task Main(string[] args)
    //        {
    //            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

    //            var hostBuilder = CreateHostBuilder(args);

    //            var isService = !(Debugger.IsAttached || args.Contains("--console"));

    //            if (isService)
    //            {
    //                var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
    //                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
    //                Directory.SetCurrentDirectory(pathToContentRoot);
    //            }

    //            if (isService)
    //                await hostBuilder.RunAsServiceAsync();
    //            else
    //                await hostBuilder.RunConsoleAsync();
    //        }

    //        private static IHostBuilder CreateHostBuilder(string[] args)
    //        {
    //            var hostBuilder = new HostBuilder()
    //                .ConfigureLogging((hostContext, config) =>
    //                {
    //                    config.AddConsole();
    //                    config.AddDebug();
    //                })
    //                .ConfigureHostConfiguration(config =>
    //                {
    //#if DEBUG
    //                    config.AddEnvironmentVariables(prefix: "ASPNETCORE_");
    //#else
    //                        config.AddEnvironmentVariables();
    //#endif
    //                })
    //                .ConfigureAppConfiguration((hostContext, config) =>
    //                {
    //                    config.SetBasePath(Directory.GetCurrentDirectory());
    //                    config.AddJsonFile("appsettings.json", false, true);
    //                    config.AddCommandLine(args);

    //#if DEBUG
    //                    config.AddUserSecrets<Program>();
    //#endif
    //                })
    //                .ConfigureServices((hostContext, services) =>
    //                {
    //                    services.AddLogging();

    //                    //configuration settings
    //                    services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));
    //                    services.Configure<RabbitMqSettings>(hostContext.Configuration.GetSection(nameof(RabbitMqSettings)));

    //                    // hosted service
    //                    services.AddHostedService<CardDataHostedService>();

    //                    services.AddApplicationServices();
    //                    services.AddInfrastructureServices();
    //                })
    //                .UseConsoleLifetime()
    //                .UseSerilog();

    //            return hostBuilder;
    //        }

    //        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs ex)
    //        {
    //            Log.Logger.Error("Unhandled exception occurred. Exception: {@Exception}", ex);
    //        }

    //    }
}
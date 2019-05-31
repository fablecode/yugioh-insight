﻿using article.cardinformation.QuartzConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using article.application.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace article.cardinformation.Services
{
    public class CardInformationWorkerService : IHostedService
    {
        public IServiceProvider Services { get; }

        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<CardInformationWorkerService> _logger;
        private readonly IHost _host;

        public CardInformationWorkerService
        (
            IServiceProvider services,
            IOptions<AppSettings> options,
            ILogger<CardInformationWorkerService> logger,
            IHost host
        )
        {
            Services = services;
            _options = options;
            _logger = logger;
            _host = host;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is starting.");
            ConfigureSerilog();
            await ConfigureQuartz(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");

            return Task.CompletedTask;
        }


        #region private helpers

        private async Task ConfigureQuartz(CancellationToken cancellationToken)
        {
            // construct a scheduler factory
            var props = new NameValueCollection
            {
                {"quartz.serializer.type", "binary"}
            };
            var factory = new StdSchedulerFactory(props);


            // define the job
            var job = JobBuilder.Create<CardInformationJob>()
                .WithIdentity("cardInformationJob", "jobGroup")
                .Build();

            // Trigger the job to run
            var trigger = TriggerBuilder.Create()
                .WithIdentity("cardInformationTrigger", "triggerGroup")
#if DEBUG
                .StartNow()
#else
                .StartNow()
                .WithCronSchedule(_options.Value.CronSchedule)
#endif
                .Build();

            // get a scheduler
            var scheduler = await factory.GetScheduler(cancellationToken);
            scheduler.JobFactory = new CardInformationJobFactory(Services);
            await scheduler.ScheduleJob(job, trigger, cancellationToken);

            await scheduler.Start(cancellationToken);

            await _host.WaitForShutdownAsync(cancellationToken);
        }

        private void ConfigureSerilog()
        {
            // Create the logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(renderMessage: true),
                    (_options.Value.LogFolder + $@"/cardinformation.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

        }

        #endregion
    }
}
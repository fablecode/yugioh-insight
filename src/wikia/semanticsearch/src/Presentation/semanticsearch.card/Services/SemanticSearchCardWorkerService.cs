﻿using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using semanticsearch.application.Configuration;
using semanticsearch.card.QuartzConfiguration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace semanticsearch.card.Services
{
    public class SemanticSearchCardWorkerService : IHostedService
    {
        public IServiceProvider Services { get; }

        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<SemanticSearchCardWorkerService> _logger;
        private IScheduler _scheduler;

        public SemanticSearchCardWorkerService
        (
            IServiceProvider services,
            IOptions<AppSettings> options,
            ILogger<SemanticSearchCardWorkerService> logger
        )
        {
            Services = services;
            _options = options;
            _logger = logger;
            ConfigureSerilog();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is starting.");
            await ConfigureQuartz(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");

            await _scheduler.Shutdown(cancellationToken);
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
            var job = CreateJob();

            // Trigger the job to run
            var trigger = CreateTrigger(_options.Value.CronSchedule);

            // get a scheduler
            _scheduler = await CreateScheduler(cancellationToken, factory, Services);

            await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            await _scheduler.Start(cancellationToken);
        }

        private static async Task<IScheduler> CreateScheduler(CancellationToken cancellationToken, ISchedulerFactory factory, IServiceProvider services)
        {
            var scheduler = await factory.GetScheduler(cancellationToken);
            scheduler.JobFactory = new SemanticSearchCardJobFactory(services);

            return scheduler;
        }

        private static ITrigger CreateTrigger(string cronSchedule)
        {
            return TriggerBuilder.Create()
                .WithIdentity("semanticSearchCardTrigger", "triggerGroup")
#if DEBUG
                .StartNow()
#else
                .StartNow()
                .WithCronSchedule(cronSchedule)
#endif
                .Build();
        }

        private static IJobDetail CreateJob()
        {
            return JobBuilder.Create<SemanticSearchCardJob>()
                .WithIdentity("semanticSearchCardJob", "jobGroup")
                .Build();
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
                    (_options.Value.LogFolder + $@"/cardsearch.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

        }

        #endregion
    }
}

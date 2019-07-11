﻿using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using article.application.Configuration;
using article.archetypes.support.cards.QuartzConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace article.archetypes.support.cards.Services
{
    public class ArchetypeWorkerService : IHostedService
    {
        public IServiceProvider Services { get; }

        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<ArchetypeWorkerService> _logger;
        private IScheduler _scheduler;

        public ArchetypeWorkerService
        (
            IServiceProvider services,
            IOptions<AppSettings> options,
            ILogger<ArchetypeWorkerService> logger
        )
        {
            Services = services;
            _options = options;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is starting.");
            ConfigureSerilog();
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
            scheduler.JobFactory = new ArchetypeInformationJobFactory(services);

            return scheduler;
        }

        private static ITrigger CreateTrigger(string cronSchedule)
        {
            return TriggerBuilder.Create()
                .WithIdentity("archetypeInformationJobTrigger", "triggerGroup")
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
            return JobBuilder.Create<ArchetypeInformationJob>()
                .WithIdentity("archetypeInformationJob", "jobGroup")
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
                    (_options.Value.LogFolder + $@"/archetypes.{Environment.MachineName}.txt"),
                    fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

        }

        #endregion
    }
}

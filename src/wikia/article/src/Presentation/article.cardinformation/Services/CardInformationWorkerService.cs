using article.cardinformation.QuartzConfiguration;
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

namespace article.cardinformation.Services
{
    public class CardInformationWorkerService : BackgroundService
    {
        public IServiceProvider Services { get; }

        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<CardInformationWorkerService> _logger;

        public CardInformationWorkerService
        (
            IServiceProvider services,
            IOptions<AppSettings> options,
            ILogger<CardInformationWorkerService> logger
        )
        {
            Services = services;
            _options = options;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is starting.");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConfigureQuartz(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"CardInformationWorkerService disposed at: {DateTime.Now}");

            base.Dispose();
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
        }

        #endregion
    }
}

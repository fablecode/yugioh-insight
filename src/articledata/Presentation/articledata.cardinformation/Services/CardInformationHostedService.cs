using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using articledata.application.Configuration;
using articledata.cardinformation.QuartzConfiguration;
using Microsoft.Extensions.Options;

namespace articledata.cardinformation.Services
{
    public class CardInformationHostedService : IHostedService
    {
        public IServiceProvider Services { get; }

        private readonly ILogger<CardInformationHostedService> _logger;
        private readonly IOptions<AppSettings> _options;

        public CardInformationHostedService
        (
            IServiceProvider services, 
            ILogger<CardInformationHostedService> logger, 
            IOptions<AppSettings> options
        )
        {
            Services = services;
            _logger = logger;
            _options = options;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is starting.");
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

            // get a scheduler
            var scheduler = await factory.GetScheduler(cancellationToken);
            scheduler.JobFactory = new CardInformationJobFactory(Services);
            await scheduler.Start(cancellationToken);

            // define the job
            var job = JobBuilder.Create<CardInformationJob>()
                .WithIdentity("cardInformationJob", "jobGroup")
                .Build();

            // Trigger the job to run
            var trigger = TriggerBuilder.Create()
                .WithIdentity("cardInformationTrigger", "triggerGroup")
#if DEBUG
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
#else
                .StartNow()
                .WithCronSchedule(_options.Value.CronSchedule)
#endif
                .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken);

            await Task.CompletedTask;
        }

        #endregion
    }
}

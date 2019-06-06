using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace semanticsearch.card.QuartzConfiguration
{
    public class SemanticSearchCardJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SemanticSearchCardJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetRequiredService<IJob>();
        }

        public void ReturnJob(IJob job) { }
    }
}
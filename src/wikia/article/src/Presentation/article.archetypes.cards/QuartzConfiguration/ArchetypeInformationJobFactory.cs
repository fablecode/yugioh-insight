using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace article.archetypes.cards.QuartzConfiguration
{
    public class ArchetypeInformationJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ArchetypeInformationJobFactory(IServiceProvider serviceProvider)
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
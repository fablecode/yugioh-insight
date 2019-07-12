using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace article.cardtrivia.QuartzConfiguration
{
    public class CardTriviaJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CardTriviaJobFactory(IServiceProvider serviceProvider)
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
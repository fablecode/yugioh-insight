using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace articledata.cardinformation.QuartzConfiguration
{
    public class CardInformationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CardInformationJob> _logger;

        public CardInformationJob(IMediator mediator, ILogger<CardInformationJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => _logger.LogInformation("Hello, Job executed"));
        }
    }
}
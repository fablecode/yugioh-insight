using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;
using semanticsearch.application.ScheduledTasks.CardSearch;

namespace semanticsearch.card.QuartzConfiguration
{
    public class SemanticSearchCardJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SemanticSearchCardJob> _logger;

        public SemanticSearchCardJob(IMediator mediator, ILogger<SemanticSearchCardJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _mediator.Send(new SemanticSearchCardTask());
        }
    }
}
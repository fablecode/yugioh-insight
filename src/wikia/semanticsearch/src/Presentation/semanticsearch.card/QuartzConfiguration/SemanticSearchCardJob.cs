using MediatR;
using Quartz;
using System.Threading.Tasks;
using semanticsearch.application.ScheduledTasks.CardSearch;

namespace semanticsearch.card.QuartzConfiguration
{
    public class SemanticSearchCardJob : IJob
    {
        private readonly IMediator _mediator;

        public SemanticSearchCardJob(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _mediator.Send(new SemanticSearchCardTask());
        }
    }
}
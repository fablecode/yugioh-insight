using article.application.ScheduledTasks.Archetype;
using MediatR;
using Quartz;
using System.Threading.Tasks;
using article.application.Configuration;
using Microsoft.Extensions.Options;

namespace article.archetypes.QuartzConfiguration
{
    [DisallowConcurrentExecution]
    public class ArchetypeInformationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly IOptions<AppSettings> _options;

        public ArchetypeInformationJob(IMediator mediator, IOptions<AppSettings> options)
        {
            _mediator = mediator;
            _options = options;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _mediator.Send(new ArchetypeInformationTask { Category = _options.Value.Category, PageSize = _options.Value.PageSize });
        }
    }
}
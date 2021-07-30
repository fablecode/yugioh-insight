using article.application.ScheduledTasks.Archetype;
using MediatR;
using Quartz;
using System.Threading.Tasks;

namespace article.archetypes.QuartzConfiguration
{
    [DisallowConcurrentExecution]
    public class ArchetypeInformationJob : IJob
    {
        private readonly IMediator _mediator;

        public ArchetypeInformationJob(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            const int pageSize = 500;
            const string category = "Archetypes";

            await _mediator.Send(new ArchetypeInformationTask { Category = category, PageSize = pageSize });
        }
    }
}
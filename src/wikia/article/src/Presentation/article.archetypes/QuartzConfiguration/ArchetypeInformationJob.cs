using System.Threading.Tasks;
using article.application.ScheduledTasks.Archetype;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace article.archetypes.QuartzConfiguration
{
    public class ArchetypeInformationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ArchetypeInformationJob> _logger;

        public ArchetypeInformationJob(IMediator mediator, ILogger<ArchetypeInformationJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            const int pageSize = 500;
            var categories = new[] { "Archetypes", "Cards by archetype", "Cards by archetype support" };

            await _mediator.Send(new ArchetypeInformationTask { Categories = categories, PageSize = pageSize });
        }
    }
}
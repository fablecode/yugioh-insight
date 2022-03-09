using article.application.ScheduledTasks.Archetype;
using MediatR;
using Quartz;
using System.Threading.Tasks;
using article.application.Configuration;
using article.domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace article.archetypes.QuartzConfiguration
{
    [DisallowConcurrentExecution]
    public class ArchetypeInformationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ArchetypeInformationJob> _logger;
        private readonly IOptions<AppSettings> _options;

        public ArchetypeInformationJob(IMediator mediator, ILogger<ArchetypeInformationJob> logger, IOptions<AppSettings> options)
        {
            _mediator = mediator;
            _logger = logger;
            _options = options;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var result = await _mediator.Send(new ArchetypeInformationTask { Category = _options.Value.Category, PageSize = _options.Value.PageSize });

            _logger.LogInformation("Finished processing '{Category}' category.", _options.Value.Category);

            if (!result.IsSuccessful)
            {
                _logger.LogInformation("Errors while processing '{Category}' category. Errors: {Errors}", _options.Value.Category, result.Errors);
            }
        }
    }
}
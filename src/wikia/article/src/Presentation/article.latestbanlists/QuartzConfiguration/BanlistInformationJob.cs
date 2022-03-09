using article.application.Configuration;
using article.application.ScheduledTasks.LatestBanlist;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System.Threading.Tasks;
using article.domain.Settings;

namespace article.latestbanlists.QuartzConfiguration
{
    public class BanlistInformationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BanlistInformationJob> _logger;
        private readonly IOptions<AppSettings> _options;

        public BanlistInformationJob(IMediator mediator, ILogger<BanlistInformationJob> logger, IOptions<AppSettings> options)
        {
            _mediator = mediator;
            _logger = logger;
            _options = options;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var result = await _mediator.Send(new BanlistInformationTask { Category = _options.Value.Category, PageSize = _options.Value.PageSize });

            _logger.LogInformation("Finished processing '{Category}' category.", _options.Value.Category);

            if (!result.IsSuccessful)
            {
                _logger.LogInformation("Errors while processing '{Category}' category. Errors: {Errors}", _options.Value.Category, result.Errors);
            }
        }
    }
}
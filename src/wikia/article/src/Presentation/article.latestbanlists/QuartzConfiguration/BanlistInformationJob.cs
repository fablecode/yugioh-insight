using System.Collections.Generic;
using System.Threading.Tasks;
using article.application.ScheduledTasks.LatestBanlist;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace article.latestbanlists.QuartzConfiguration
{
    public class BanlistInformationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BanlistInformationJob> _logger;

        public BanlistInformationJob(IMediator mediator, ILogger<BanlistInformationJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            const int pageSize = 500;
            const string category = "Forbidden & Limited Lists";

            await _mediator.Send(new BanlistInformationTask { Category = category, PageSize = pageSize });
        }
    }
}
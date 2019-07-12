using System.Threading.Tasks;
using article.application.ScheduledTasks.Articles;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace article.cardtips.QuartzConfiguration
{
    public class CardTipsJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CardTipsJob> _logger;

        public CardTipsJob(IMediator mediator, ILogger<CardTipsJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            const int pageSize = 500;
            const string category = "Card Tips";

            await _mediator.Send(new ArticleInformationTask { Category = category, PageSize = pageSize });
        }
    }
}
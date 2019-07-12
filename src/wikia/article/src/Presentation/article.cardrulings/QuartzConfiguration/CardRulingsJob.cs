using System.Threading.Tasks;
using article.application.ScheduledTasks.Articles;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace article.cardrulings.QuartzConfiguration
{
    public class CardRulingsJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CardRulingsJob> _logger;

        public CardRulingsJob(IMediator mediator, ILogger<CardRulingsJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            const int pageSize = 500;
            const string category = "Card Rulings";

            await _mediator.Send(new ArticleInformationTask { Category = category, PageSize = pageSize });
        }
    }
}
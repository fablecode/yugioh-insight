using System.Threading.Tasks;
using article.application.ScheduledTasks.Articles;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace article.cardtrivia.QuartzConfiguration
{
    public class CardTriviaJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CardTriviaJob> _logger;

        public CardTriviaJob(IMediator mediator, ILogger<CardTriviaJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            const int pageSize = 500;
            const string category = "Card Trivia";

            await _mediator.Send(new ArticleInformationTask { Category = category, PageSize = pageSize });
        }
    }
}
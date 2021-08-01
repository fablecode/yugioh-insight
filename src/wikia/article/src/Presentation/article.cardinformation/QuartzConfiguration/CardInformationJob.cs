using article.application.Configuration;
using article.application.ScheduledTasks.Articles;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System.Threading.Tasks;

namespace article.cardinformation.QuartzConfiguration
{
    public class CardInformationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CardInformationJob> _logger;
        private readonly IOptions<AppSettings> _options;

        public CardInformationJob(IMediator mediator, ILogger<CardInformationJob> logger, IOptions<AppSettings> options)
        {
            _mediator = mediator;
            _logger = logger;
            _options = options;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //const int pageSize = 500;
            //const string tcgCards = "TCG cards";
            //const string ocgCards = "OCG cards";

            //var categories = new List<string> { tcgCards, ocgCards };

            var result = await _mediator.Send(new ArticleInformationTask { Category = _options.Value.Category, PageSize = _options.Value.PageSize });

            _logger.LogInformation("Finished processing '{Category}' category.", _options.Value.Category);

            if (!result.IsSuccessful)
            {
                _logger.LogInformation("Errors while processing '{Category}' category. Errors: {Errors}", _options.Value.Category, result.Errors);
            }
        }
    }
}
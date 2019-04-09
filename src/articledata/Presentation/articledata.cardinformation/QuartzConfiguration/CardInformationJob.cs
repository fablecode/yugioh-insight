using System.Collections.Generic;
using System.Threading.Tasks;
using articledata.application.ScheduledTasks.CardInformation;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace articledata.cardinformation.QuartzConfiguration
{
    public class CardInformationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CardInformationJob> _logger;

        public CardInformationJob(IMediator mediator, ILogger<CardInformationJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            const int pageSize = 500;
            const string tcgCards = "TCG cards";
            const string ocgCards = "OCG cards";

            var categories = new List<string> { tcgCards, ocgCards };

            await _mediator.Send(new CardInformationTask { Categories = categories, PageSize = pageSize });
        }
    }
}
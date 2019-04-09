using System.Collections.Generic;
using MediatR;

namespace articledata.application.ScheduledTasks.CardInformation
{
    public class CardInformationTask : IRequest<CardInformationTaskResult>
    {
        public List<string> Categories { get; set; }

        public int PageSize { get; set; }
    }
}
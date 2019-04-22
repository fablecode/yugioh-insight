using System.Collections.Generic;
using MediatR;

namespace articledata.application.ScheduledTasks.CardInformation
{
    public class CardInformationTask : IRequest<CardInformationTaskResult>
    {
        public Dictionary<string, object> Headers { get; set; }

        public string Message { get; set; }
    }
}
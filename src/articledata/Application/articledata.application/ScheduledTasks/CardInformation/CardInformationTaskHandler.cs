using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace articledata.application.ScheduledTasks.CardInformation
{
    public class CardInformationTaskHandler : IRequestHandler<CardInformationTask, CardInformationTaskResult>
    {
        public Task<CardInformationTaskResult> Handle(CardInformationTask request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
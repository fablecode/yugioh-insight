using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace article.application.ScheduledTasks.Archetype
{
    public class ArchetypeInformationTaskHandler : IRequestHandler<ArchetypeInformationTask, ArchetypeInformationTaskResult>
    {
        public Task<ArchetypeInformationTaskResult> Handle(ArchetypeInformationTask request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
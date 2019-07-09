using MediatR;

namespace article.application.ScheduledTasks.Archetype
{
    public class ArchetypeInformationTask : IRequest<ArchetypeInformationTaskResult>
    {
        public string[] Categories { get; set; }
        public int PageSize { get; set; }
    }
}
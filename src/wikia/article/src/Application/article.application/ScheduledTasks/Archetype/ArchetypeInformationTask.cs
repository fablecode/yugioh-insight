using MediatR;

namespace article.application.ScheduledTasks.Archetype
{
    public class ArchetypeInformationTask : IRequest<ArchetypeInformationTaskResult>
    {
        public string Category { get; set; }
        public int PageSize { get; set; }
    }
}
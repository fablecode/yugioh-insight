using MediatR;

namespace article.application.ScheduledTasks.LatestBanlist
{
    public class BanlistInformationTask : IRequest<BanlistInformationTaskResult>
    {
        public string Category { get; set; }

        public int PageSize { get; set; }
    }
}
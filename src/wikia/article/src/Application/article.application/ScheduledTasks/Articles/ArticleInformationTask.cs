using MediatR;

namespace article.application.ScheduledTasks.Articles
{
    public class ArticleInformationTask : IRequest<ArticleInformationTaskResult>
    {
        public string Category { get; set; }
        public int PageSize { get; set; }
    }
}
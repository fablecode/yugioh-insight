using article.core.Models;
using System.Collections.Generic;

namespace article.application.ScheduledTasks.Articles
{
    public class ArticleInformationTaskResult
    {
        public ArticleBatchTaskResult ArticleTaskResults { get; set; }

        public List<string> Errors { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
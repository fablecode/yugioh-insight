using System.Collections.Generic;
using article.core.Models;

namespace article.application.ScheduledTasks.CardInformation
{
    public class CardInformationTaskResult
    {
        public IEnumerable<ArticleBatchTaskResult> ArticleTaskResults { get; set; } = new List<ArticleBatchTaskResult>();

        public List<string> Errors { get; set; }
    }
}
using System.Collections.Generic;
using articledata.core.Models;

namespace articledata.application.ScheduledTasks.CardInformation
{
    public class CardInformationTaskResult
    {
        public IEnumerable<ArticleBatchTaskResult> ArticleTaskResults { get; set; } = new List<ArticleBatchTaskResult>();

        public List<string> Errors { get; set; }
    }
}
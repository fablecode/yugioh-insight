using System.Collections.Generic;
using articledata.domain.Model;

namespace articledata.application.ScheduledTasks.CardInformation
{
    public class CardInformationTaskResult
    {
        public List<ArticleBatchTaskResult> ArticleTaskResults { get; set; } = new List<ArticleBatchTaskResult>();

        public List<string> Errors { get; set; }
    }
}
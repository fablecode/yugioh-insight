using System.Collections.Generic;
using article.core.Models;

namespace article.application.ScheduledTasks.LatestBanlist
{
    public class BanlistInformationTaskResult
    {
        public ArticleBatchTaskResult ArticleTaskResults { get; set; }

        public List<string> Errors { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
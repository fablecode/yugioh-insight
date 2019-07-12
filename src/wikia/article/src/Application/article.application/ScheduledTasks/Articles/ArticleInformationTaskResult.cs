using System.Collections.Generic;
using article.application.ScheduledTasks.LatestBanlist;
using article.core.Enums;
using article.core.Models;
using article.domain.Banlist.Processor;

namespace article.application.ScheduledTasks.Articles
{
    public class ArticleInformationTaskResult
    {
        public ArticleBatchTaskResult ArticleTaskResults { get; set; }

        public List<string> Errors { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
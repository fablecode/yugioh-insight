using article.core.Models;
using System.Collections.Generic;

namespace article.application.ScheduledTasks.Archetype
{
    public class ArchetypeInformationTaskResult
    {
        public List<string> Errors { get; set; }
        public bool IsSuccessful { get; set; }
        public IEnumerable<ArticleBatchTaskResult> ArticleTaskResults { get; set; } = new List<ArticleBatchTaskResult>();
    }
}
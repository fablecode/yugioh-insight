using System.Collections.Generic;

namespace cardsectiondata.core.Models
{
    public class ArticleTaskResult
    {
        public bool IsSuccessful { get; set; }

        public Article Article { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
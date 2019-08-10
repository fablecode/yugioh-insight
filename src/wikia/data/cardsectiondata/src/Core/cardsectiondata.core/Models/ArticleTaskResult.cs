using System.Collections.Generic;
using System.Linq;

namespace cardsectiondata.core.Models
{
    public class ArticleTaskResult
    {
        public bool IsSuccessful => !Errors.Any();

        public Article Article { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
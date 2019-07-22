using System.Collections.Generic;
using System.Linq;

namespace archetypeprocessor.core.Models
{
    public class ArticleDataTaskResult
    {
        public bool IsSuccessful => !Errors.Any();

        public ArticleData ArticleData { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
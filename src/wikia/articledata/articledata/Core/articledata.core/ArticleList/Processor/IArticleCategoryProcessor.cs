using System.Collections.Generic;
using System.Threading.Tasks;
using articledata.core.Models;

namespace articledata.core.ArticleList.Processor
{
    public interface IArticleCategoryProcessor
    {
        Task<IEnumerable<ArticleBatchTaskResult>> Process(IEnumerable<string> categories, int pageSize);
        Task<ArticleBatchTaskResult> Process(string category, int pageSize);
    }
}
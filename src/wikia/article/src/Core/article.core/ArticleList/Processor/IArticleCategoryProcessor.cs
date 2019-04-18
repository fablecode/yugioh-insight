using System.Collections.Generic;
using System.Threading.Tasks;
using article.core.Models;

namespace article.core.ArticleList.Processor
{
    public interface IArticleCategoryProcessor
    {
        Task<IEnumerable<ArticleBatchTaskResult>> Process(IEnumerable<string> categories, int pageSize);
        Task<ArticleBatchTaskResult> Process(string category, int pageSize);
    }
}
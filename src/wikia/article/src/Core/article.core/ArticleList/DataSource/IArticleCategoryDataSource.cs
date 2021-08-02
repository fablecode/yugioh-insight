using System.Collections.Generic;
using wikia.Models.Article.AlphabeticalList;

namespace article.core.ArticleList.DataSource
{
    public interface IArticleCategoryDataSource
    {
        IAsyncEnumerable<UnexpandedArticle[]> Producer(string category, int pageSize);
    }
}
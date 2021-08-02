using article.core.Models;
using article.domain.Services.Messaging;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.Services.Messaging
{
    public class ArchetypeArticleQueue : IArchetypeArticleQueue
    {
        private readonly IQueue<Article> _queue;

        public ArchetypeArticleQueue(IQueue<Article> queue)
        {
            _queue = queue;
        }

        public Task Publish(UnexpandedArticle article)
        {
            return _queue.Publish(article);
        }
    }
}
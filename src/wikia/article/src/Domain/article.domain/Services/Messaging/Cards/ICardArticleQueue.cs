using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.Services.Messaging.Cards
{
    public interface ICardArticleQueue
    {
        Task Publish(UnexpandedArticle article);
    }
}
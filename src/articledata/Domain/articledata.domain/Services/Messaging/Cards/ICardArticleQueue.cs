using System.Threading.Tasks;
using articledata.domain.Contracts;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.Services.Messaging.Cards
{
    public interface ICardArticleQueue
    {
        Task Publish(UnexpandedArticle article);
    }
}
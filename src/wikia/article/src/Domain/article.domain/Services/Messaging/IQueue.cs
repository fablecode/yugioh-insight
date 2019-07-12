using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.Services.Messaging
{
    public interface IQueue<in T>
    {
        Task Publish(UnexpandedArticle message);
        Task Publish(T message);
    }
}
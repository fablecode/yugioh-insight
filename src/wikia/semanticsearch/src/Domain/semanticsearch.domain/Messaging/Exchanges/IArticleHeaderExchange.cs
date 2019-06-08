using System.Threading.Tasks;
using semanticsearch.core.Model;

namespace semanticsearch.domain.Messaging.Exchanges
{
    public interface IArticleHeaderExchange
    {
        Task Publish(SemanticCard semanticCard);
    }
}
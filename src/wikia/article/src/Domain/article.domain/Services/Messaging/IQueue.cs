using System.Threading.Tasks;

namespace article.domain.Services.Messaging
{
    public interface IQueue<in T>
    {
        Task Publish(T message);
    }
}
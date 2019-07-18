using System.Threading.Tasks;

namespace archetypedata.domain.Services.Messaging
{
    public interface IQueue<in T>
    {
        Task Publish(T message);
    }
}
using System.Threading.Tasks;
using cardsectiondata.core.Models;

namespace cardsectiondata.domain.Services.Messaging
{
    public interface IQueue
    {
        Task Publish(CardSectionMessage message);

        bool Handles(string category);
    }
}
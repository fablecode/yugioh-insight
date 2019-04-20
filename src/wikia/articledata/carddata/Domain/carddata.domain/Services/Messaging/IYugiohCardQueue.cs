using System.Threading.Tasks;
using carddata.core.Models;

namespace carddata.domain.Services.Messaging
{
    public interface IYugiohCardQueue
    {
        Task Publish(YugiohCard yugiohCard);
    }
}
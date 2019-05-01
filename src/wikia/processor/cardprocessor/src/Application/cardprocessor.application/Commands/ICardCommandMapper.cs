using System.Threading.Tasks;
using cardprocessor.application.Commands.AddCard;
using cardprocessor.application.Commands.UpdateCard;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Commands
{
    public interface ICardCommandMapper
    {
        Task<AddCardCommand> MapToAddCommand(YugiohCard yugiohCard);
        Task<UpdateCardCommand> MapToUpdateCommand(YugiohCard yugiohCard, Card card);
    }
}
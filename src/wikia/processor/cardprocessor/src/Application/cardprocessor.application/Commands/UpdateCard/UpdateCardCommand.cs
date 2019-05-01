using cardprocessor.application.Models.Cards.Input;
using MediatR;

namespace cardprocessor.application.Commands.UpdateCard
{
    public class UpdateCardCommand : IRequest<CommandResult>
    {
        public CardInputModel Card { get; set; }
    }
}
using cardprocessor.application.Models.Cards.Input;
using MediatR;

namespace cardprocessor.application.Commands.AddCard
{
    public class AddCardCommand : IRequest<CommandResult>
    {
        public CardInputModel Card { get; set; }
    }
}
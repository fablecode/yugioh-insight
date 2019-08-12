using System.Threading.Tasks;
using cardsectionprocessor.core.Models;

namespace cardsectionprocessor.core.Strategy
{
    public interface ICardSectionProcessorStrategy
    {
        Task<CardSectionDataTaskResult<CardSectionMessage>> Process(CardSectionMessage cardSectionData);

        bool Handles(string category);
    }
}
using System.Threading.Tasks;
using cardsectionprocessor.core.Models;

namespace cardsectionprocessor.core.Processor
{
    public interface ICardSectionProcessor
    {
        Task<CardSectionDataTaskResult<CardSectionMessage>> Process(string category, CardSectionMessage cardSectionData);
    }
}
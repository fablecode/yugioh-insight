using System.Threading.Tasks;
using cardsectiondata.core.Models;

namespace cardsectiondata.core.Processor
{
    public interface ICardSectionProcessor
    {
        Task<CardSectionMessage> ProcessItem(Article article);
    }
}
using cardsectiondata.core.Models;

namespace cardsectiondata.domain.WebPages
{
    public interface ITipRelatedWebPage
    {
        void GetTipRelatedCards(CardSection section, Article article);
    }
}
using System;
using cardprocessor.application.Enums;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;

namespace cardprocessor.application.Helpers.Cards
{
    public static class CardHelper
    {
        public static CardInputModel MapCardImageUrl(YugiohCard yugiohCard, CardInputModel cardInputModel)
        {
            if (yugiohCard.ImageUrl != null)
                cardInputModel.ImageUrl = new Uri(yugiohCard.ImageUrl);

            return cardInputModel;
        }

        public static CardInputModel MapBasicCardInformation(YugiohCard yugiohCard, CardInputModel cardInputModel)
        {
            cardInputModel.CardType =
                (YgoCardType?)(Enum.TryParse(typeof(YgoCardType), yugiohCard.CardType, true, out var cardType)
                    ? cardType
                    : null);
            cardInputModel.CardNumber = long.TryParse(yugiohCard.CardNumber, out var cardNumber) ? cardNumber : (long?)null;
            cardInputModel.Name = yugiohCard.Name;
            cardInputModel.Description = yugiohCard.Description;

            return cardInputModel;
        }
    }
}
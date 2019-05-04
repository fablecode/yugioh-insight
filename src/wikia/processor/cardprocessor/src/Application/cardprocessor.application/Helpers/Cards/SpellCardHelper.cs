using System.Collections.Generic;
using cardprocessor.application.Enums;
using cardprocessor.application.Mappings.Mappers;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Helpers.Cards
{
    public static class SpellCardHelper
    {
        public static CardInputModel MapSubCategoryIds(YugiohCard yugiohCard, CardInputModel cardInputModel, ICollection<Category> categories, ICollection<SubCategory> subCategories)
        {
            cardInputModel.SubCategoryIds = new List<int>
            {
                SubCategoryMapper.SubCategoryId(categories, subCategories, YgoCardType.Spell, yugiohCard.Property)
            };

            return cardInputModel;
        }
    }
}
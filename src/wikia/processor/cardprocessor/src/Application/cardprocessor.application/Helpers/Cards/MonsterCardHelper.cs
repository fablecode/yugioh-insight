using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using Attribute = cardprocessor.core.Models.Db.Attribute;
using Type = cardprocessor.core.Models.Db.Type;

namespace cardprocessor.application.Helpers.Cards
{
    public class MonsterCardHelper
    {
        public static List<int> MonsterLinkArrowIds(YugiohCard yugiohCard, ICollection<LinkArrow> linkArrows)
        {
            return linkArrows
                .Where(mla => yugiohCard.MonsterLinkArrows.Any(ymla => ymla.Equals(mla.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(la => (int)la.Id)
                .ToList();
        }

        public static List<int> MonsterTypeIds(YugiohCard yugiohCard, ICollection<Type> types)
        {
            return types
                .Where(mt => yugiohCard.MonsterSubCategoriesAndTypes.Any(yt => yt.Equals(mt.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(smt => (int)smt.Id)
                .ToList();
        }

        public static List<int> MonsterSubCategoryIds(YugiohCard yugiohCard, IEnumerable<SubCategory> monsterSubCategories)
        {
            return monsterSubCategories
                .Where(msb => yugiohCard.MonsterSubCategoriesAndTypes.Any(ysc => ysc.Equals(msb.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(ssc => (int)ssc.Id)
                .ToList();
        }

        public static int MonsterAttributeId(YugiohCard yugiohCard, ICollection<Attribute> attributes)
        {
            return
                (int) 
                (
                    from a in attributes
                    where a.Name.Equals(yugiohCard.Attribute, StringComparison.OrdinalIgnoreCase)
                    select a.Id
                )
                .Single();
        }

        public static string Atk(YugiohCard yugiohCard)
        {
            return yugiohCard.AtkDef?.Split('/').First().Trim();
        }
        public static string AtkLink(YugiohCard yugiohCard)
        {
            return yugiohCard.AtkLink?.Split('/').First().Trim();
        }

        public static string DefOrLink(YugiohCard yugiohCard)
        {
            return yugiohCard.AtkDef?.Split('/').Last().Trim();
        }

        public static CardInputModel MapMonsterCard(YugiohCard yugiohCard, CardInputModel cardInputModel, ICollection<Attribute> attributes, IEnumerable<SubCategory> monsterSubCategories, ICollection<Type> types, ICollection<LinkArrow> linkArrows)
        {
            cardInputModel.AttributeId = MonsterAttributeId(yugiohCard, attributes);
            cardInputModel.SubCategoryIds = MonsterSubCategoryIds(yugiohCard, monsterSubCategories);
            cardInputModel.TypeIds = MonsterTypeIds(yugiohCard, types);

            if (yugiohCard.LinkArrows != null)
            {
                cardInputModel.LinkArrowIds = MonsterLinkArrowIds(yugiohCard, linkArrows);
            }

            if (yugiohCard.Level.HasValue)
                cardInputModel.CardLevel = yugiohCard.Level;

            if (yugiohCard.Rank.HasValue)
                cardInputModel.CardRank = yugiohCard.Rank;

            if (!string.IsNullOrWhiteSpace(yugiohCard.AtkDef))
            {
                var atk = Atk(yugiohCard);
                var def = DefOrLink(yugiohCard);

                int.TryParse(atk, out var cardAtk);
                int.TryParse(def, out var cardDef);

                cardInputModel.Atk = cardAtk;
                cardInputModel.Def = cardDef;
            }

            if (!string.IsNullOrWhiteSpace(yugiohCard.AtkLink))
            {
                var atk = AtkLink(yugiohCard);

                int.TryParse(atk, out var cardAtk);

                cardInputModel.Atk = cardAtk;
            }

            return cardInputModel;
        }

    }
}
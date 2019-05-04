using System;
using System.Collections.Generic;
using System.Linq;
using cardprocessor.application.Commands.AddCard;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;
using Attribute = cardprocessor.core.Models.Db.Attribute;
using Type = cardprocessor.core.Models.Db.Type;

namespace cardprocessor.application.Mappings.Mappers
{
    public class MonsterCardCommandMapper
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
            return yugiohCard.AtkDef?.Split('/').First();
        }
        public static string AtkLink(YugiohCard yugiohCard)
        {
            return yugiohCard.AtkLink?.Split('/').First();
        }

        public static string DefOrLink(YugiohCard yugiohCard)
        {
            return yugiohCard.AtkDef?.Split('/').Last();
        }

        public static void MapMonsterCard(YugiohCard yugiohCard, AddCardCommand command, ICollection<Attribute> attributes, IEnumerable<SubCategory> monsterSubCategories, ICollection<Type> types, ICollection<LinkArrow> linkArrows)
        {
            command.Card.AttributeId = MonsterAttributeId(yugiohCard, attributes);

            command.Card.SubCategoryIds = MonsterSubCategoryIds(yugiohCard, monsterSubCategories);
            command.Card.TypeIds = MonsterTypeIds(yugiohCard, types);

            if (yugiohCard.LinkArrows != null)
            {
                command.Card.LinkArrowIds = MonsterLinkArrowIds(yugiohCard, linkArrows);
            }


            if (yugiohCard.Level.HasValue)
                command.Card.CardLevel = yugiohCard.Level;

            if (yugiohCard.Rank.HasValue)
                command.Card.CardRank = yugiohCard.Rank;

            if (!string.IsNullOrWhiteSpace(yugiohCard.AtkDef))
            {
                var atk = Atk(yugiohCard);
                var def = DefOrLink(yugiohCard);

                int.TryParse(atk, out var cardAtk);
                int.TryParse(def, out var cardDef);

                command.Card.Atk = cardAtk;
                command.Card.Def = cardDef;
            }

            if (!string.IsNullOrWhiteSpace(yugiohCard.AtkLink))
            {
                var atk = AtkLink(yugiohCard);

                int.TryParse(atk, out var cardAtk);

                command.Card.Atk = cardAtk;
            }
        }

    }
}
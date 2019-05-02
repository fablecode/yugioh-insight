using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string DefOrLink(YugiohCard yugiohCard)
        {
            return yugiohCard.AtkDef?.Split('/').Last();
        }
    }
}
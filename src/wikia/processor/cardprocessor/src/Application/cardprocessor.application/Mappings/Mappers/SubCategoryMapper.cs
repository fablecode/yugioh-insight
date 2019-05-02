using System;
using System.Collections.Generic;
using System.Linq;
using cardprocessor.application.Enums;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.Mappings.Mappers
{
    public class SubCategoryMapper
    {
        public static int SubCategoryId(IEnumerable<Category> categories, IEnumerable<SubCategory> subCategories, YgoCardType cardType, string subCategory)
        {
            return
                (int)
                (
                    from sc in subCategories
                    join c in categories on sc.CategoryId equals c.Id
                    where c.Name.Equals(cardType.ToString(), StringComparison.OrdinalIgnoreCase) && sc.Name.Equals(subCategory, StringComparison.OrdinalIgnoreCase)
                    select sc.Id
                )
                .Single();
        }
    }
}
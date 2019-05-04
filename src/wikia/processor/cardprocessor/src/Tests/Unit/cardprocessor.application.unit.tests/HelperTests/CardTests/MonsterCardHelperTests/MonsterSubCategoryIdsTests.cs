using System;
using System.Collections.Generic;
using System.Linq;
using cardprocessor.application.Enums;
using cardprocessor.application.Helpers.Cards;
using cardprocessor.core.Models;
using cardprocessor.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.HelperTests.CardTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MonsterSubCategoryIdsTests
    {
        [Test]
        public void Given_A_Valid_A_Link_Monster_YugiohCard_With_Types_Should_Map_To_SubCategoryIds_Property()
        {
            // Arrange
            var expected = new List<int> { 2, 14 };

            var yugiohCard = new YugiohCard
            {
                Name = "Decode Talker",
                Description = "Amazing card!",
                CardNumber = "01861629",
                Attribute = "Dark",
                Types = "Cyberse / Link / Effect",
                CardType = "Monster",
                LinkArrows = "	Bottom-Left, Top, Bottom-Right",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/5/5d/DecodeTalker-YS18-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180712163921"
            };
            var monsterCategory = TestData.AllCategories().Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
            var monsterSubCategories = TestData.AllSubCategories().Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

            // Act
            var result = MonsterCardHelper.MonsterSubCategoryIds(yugiohCard, monsterSubCategories);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using cardprocessor.application.Enums;
using cardprocessor.application.Helpers.Cards;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.HelperTests.CardTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MapMonsterCardTests
    {
        [Test]
        public void Given_A_Valid_A_Monster_YugiohCard_Should_Map_To_CardInputModel()
        {
            // Arrange
            var yugiohCard = new YugiohCard
            {
                Name = "Decode Talker",
                Description = "Amazing card!",
                CardNumber = "01861629",
                Attribute = "Dark",
                Types = "Cyberse / Link / Effect",
                CardType = "Monster",
                LinkArrows = "	Bottom-Left, Top, Bottom-Right",
                AtkDef = "2300 / 3",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/5/5d/DecodeTalker-YS18-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180712163921"
            };
            var cardInputModel = new CardInputModel();
            var monsterCategory = TestData.AllCategories().Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
            var monsterSubCategories = TestData.AllSubCategories().Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

            // Act
            var result = MonsterCardHelper.MapMonsterCard(yugiohCard, cardInputModel, TestData.AllAttributes(), monsterSubCategories, TestData.AllTypes(), TestData.AllLinkArrows());

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Given_A_Valid_YugiohCard_With_Types_Should_Map_To_TypeIds_Property()
        {
            // Arrange
            var expected = new List<int> { 8 };

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            var cardInputModel = new CardInputModel();
            var monsterCategory = TestData.AllCategories().Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
            var monsterSubCategories = TestData.AllSubCategories().Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

            // Act
            var result = MonsterCardHelper.MapMonsterCard(yugiohCard, cardInputModel, TestData.AllAttributes(), monsterSubCategories, TestData.AllTypes(), TestData.AllLinkArrows());

            // Assert
            result.TypeIds.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_A_Valid_YugiohCard_With_A_Level_Should_Map_To_CardLevel_Property()
        {
            // Arrange
            const int expected = 8;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Level = 8,
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            var cardInputModel = new CardInputModel();
            var monsterCategory = TestData.AllCategories().Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
            var monsterSubCategories = TestData.AllSubCategories().Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

            // Act
            var result = MonsterCardHelper.MapMonsterCard(yugiohCard, cardInputModel, TestData.AllAttributes(), monsterSubCategories, TestData.AllTypes(), TestData.AllLinkArrows());

            // Assert
            result.CardLevel.Should().Be(expected);
        }

        [Test]
        public void Given_A_Valid_YugiohCard_With_A_Rank_Should_Map_To_CardRank_Property()
        {
            // Arrange
            const int expected = 3;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Rank = 3,
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            var cardInputModel = new CardInputModel();
            var monsterCategory = TestData.AllCategories().Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
            var monsterSubCategories = TestData.AllSubCategories().Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

            // Act
            var result = MonsterCardHelper.MapMonsterCard(yugiohCard, cardInputModel, TestData.AllAttributes(), monsterSubCategories, TestData.AllTypes(), TestData.AllLinkArrows());

            // Assert
            result.CardRank.Should().Be(expected);
        }

        [Test]
        public void Given_A_Valid_YugiohCard_With_An_Atk_Should_Map_To_Atk_Property()
        {
            // Arrange
            const int expected = 3000;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Level = 8,
                AtkDef = "3000 / 2500",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            var cardInputModel = new CardInputModel();
            var monsterCategory = TestData.AllCategories().Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
            var monsterSubCategories = TestData.AllSubCategories().Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

            // Act
            var result = MonsterCardHelper.MapMonsterCard(yugiohCard, cardInputModel, TestData.AllAttributes(), monsterSubCategories, TestData.AllTypes(), TestData.AllLinkArrows());

            // Assert
            result.Atk.Should().Be(expected);
        }

        [Test]
        public void Given_A_Valid_Link_Monster_YugiohCard_With_An_Atk_Should_Map_To_Atk_Property()
        {
            // Arrange
            const int expected = 2300;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Rank = 3,
                AtkLink = "2300 / 3",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            var cardInputModel = new CardInputModel();
            var monsterCategory = TestData.AllCategories().Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
            var monsterSubCategories = TestData.AllSubCategories().Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

            // Act
            var result = MonsterCardHelper.MapMonsterCard(yugiohCard, cardInputModel, TestData.AllAttributes(), monsterSubCategories, TestData.AllTypes(), TestData.AllLinkArrows());

            // Assert
            result.Atk.Should().Be(expected);
        }

        [Test]
        public void Given_A_Valid_YugiohCard_With_An_Def_Should_Map_To_Def_Property()
        {
            // Arrange
            const int expected = 2500;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Level = 8,
                AtkDef = "3000 / 2500",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            var cardInputModel = new CardInputModel();
            var monsterCategory = TestData.AllCategories().Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
            var monsterSubCategories = TestData.AllSubCategories().Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

            // Act
            var result = MonsterCardHelper.MapMonsterCard(yugiohCard, cardInputModel, TestData.AllAttributes(), monsterSubCategories, TestData.AllTypes(), TestData.AllLinkArrows());

            // Assert
            result.Def.Should().Be(expected);
        }

    }
}
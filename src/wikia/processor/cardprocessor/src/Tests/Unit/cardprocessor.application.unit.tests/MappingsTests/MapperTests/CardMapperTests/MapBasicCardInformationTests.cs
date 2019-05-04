using cardprocessor.application.Enums;
using cardprocessor.application.Helpers.Cards;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.MappingsTests.MapperTests.CardMapperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MapBasicCardInformationTests
    {
        [TestCase("Spell", YgoCardType.Spell)]
        [TestCase("Trap", YgoCardType.Trap)]
        [TestCase("speLl", YgoCardType.Spell)]
        [TestCase("traP", YgoCardType.Trap)]
        public void Given_A_Valid_SpellOrTrap_CardType_Should_Map_CardType_To_Correct_YgoCardType(string cardType, YgoCardType expected)
        {
            // Arrange
            var yugiohCard = new YugiohCard
            {
                Property = "Normal",
                CardType = cardType
            };
            var cardInputModel = new CardInputModel();

            // Act
            var result = CardHelper.MapBasicCardInformation(yugiohCard, cardInputModel);

            // Assert
            result.CardType.Should().Be(expected);
        }

        [TestCase("Monster", YgoCardType.Monster)]
        [TestCase("monsTer", YgoCardType.Monster)]
        public void Given_A_Valid_Monster_CardType_Should_Map_CardType_To_Correct_YgoCardType(string cardType, YgoCardType expected)
        {
            // Arrange
            var yugiohCard = new YugiohCard
            {
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = cardType
            };
            var cardInputModel = new CardInputModel();

            // Act
            var result = CardHelper.MapBasicCardInformation(yugiohCard, cardInputModel);

            // Assert
            result.CardType.Should().Be(expected);
        }

        [Test]
        public void Given_A_Valid_YugiohCard_With_A_Name_Should_Map_To_CardInputModel_Name_Property()
        {
            // Arrange
            const string expected = "Darkness Dragon";

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };
            var cardInputModel = new CardInputModel();

            // Act
            var result = CardHelper.MapBasicCardInformation(yugiohCard, cardInputModel);

            // Assert
            result.Name.Should().Be(expected);
        }

        [Test]
        public void Given_A_Valid_YugiohCard_With_A_Description_Should_Map_To_CardInputModel_Description_Property()
        {
            // Arrange
            const string expected = "Amazing card!";

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };
            var cardInputModel = new CardInputModel();

            // Act
            var result = CardHelper.MapBasicCardInformation(yugiohCard, cardInputModel);

            // Assert
            result.Description.Should().Be(expected);
        }

    }
}
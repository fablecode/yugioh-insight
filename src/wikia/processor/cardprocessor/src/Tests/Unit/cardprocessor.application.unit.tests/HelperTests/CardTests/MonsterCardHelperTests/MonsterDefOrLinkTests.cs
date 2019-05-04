using cardprocessor.application.Helpers.Cards;
using cardprocessor.core.Models;
using cardprocessor.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.HelperTests.CardTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MonsterDefOrLinkTests
    {
        [Test]
        public void Given_A_Valid_A_Monster_YugiohCard_Should_Extract_Def_Value()
        {
            // Arrange
            const string expected = "500";

            var yugiohCard = new YugiohCard
            {
                Name = "Decode Talker",
                Description = "Amazing card!",
                CardNumber = "01861629",
                Attribute = "Dark",
                Types = "Cyberse / Link / Effect",
                CardType = "Monster",
                LinkArrows = "	Bottom-Left, Top, Bottom-Right",
                AtkDef = "2300 / 500",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/5/5d/DecodeTalker-YS18-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180712163921"
            };

            // Act
            var result = MonsterCardHelper.DefOrLink(yugiohCard);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Given_A_Valid_A_Link_Monster_YugiohCard_Should_Extract_Link_Value()
        {
            // Arrange
            const string expected = "3";

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

            // Act
            var result = MonsterCardHelper.DefOrLink(yugiohCard);

            // Assert
            result.Should().Be(expected);
        }
    }
}
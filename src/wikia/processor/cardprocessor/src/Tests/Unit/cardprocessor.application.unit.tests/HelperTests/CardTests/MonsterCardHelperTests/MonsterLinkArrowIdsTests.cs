using System.Collections.Generic;
using cardprocessor.application.Helpers.Cards;
using cardprocessor.core.Models;
using cardprocessor.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.HelperTests.CardTests.MonsterCardHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MonsterLinkArrowIdsTests
    {
        [Test]
        public void Given_A_Valid_A_Link_Monster_YugiohCard_With_LinkArrows_Should_Map_To_LinkArrowIds_Property()
        {
            // Arrange
            var expected = new List<int> { 7, 2, 5};

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

            // Act
            var result = MonsterCardHelper.MonsterLinkArrowIds(yugiohCard, TestData.AllLinkArrows());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
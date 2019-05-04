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
    public class MapCardImageUrlTests
    {
        [Test]
        public void Given_A_Valid_YugiohCard_With_An_ImageUrl_Should_Map_To_CardInputModel_ImageUrl_Property()
        {
            // Arrange
            const string expected = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125";

            var yugiohCard = new YugiohCard
            {
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };
            var cardInputModel = new CardInputModel();

            // Act
            var result = CardHelper.MapCardImageUrl(yugiohCard, cardInputModel);

            // Assert
            result.ImageUrl.AbsoluteUri.Should().BeEquivalentTo(expected);
        }
    }
}
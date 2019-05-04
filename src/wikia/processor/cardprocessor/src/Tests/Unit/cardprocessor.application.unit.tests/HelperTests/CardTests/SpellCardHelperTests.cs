using System.Collections.Generic;
using cardprocessor.application.Helpers.Cards;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.HelperTests.CardTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class SpellCardHelperTests
    {
        [Test]
        public void Given_A_Valid_Spell_YugiohCard_With_Types_Should_Map_To_SubCategoryIds_Property()
        {
            // Arrange
            var expected = new List<int> { 18 };

            var yugiohCard = new YugiohCard
            {
                Name = "Black Illusion Ritual",

                Description = "Amazing card!",
                CardNumber = "41426869",
                Property = "Ritual",
                CardType = "Spell",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/8/82/BlackIllusionRitual-LED2-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180223153750"
            };
            var cardInputModel = new CardInputModel();

            // Act
            var result = SpellCardHelper.MapSubCategoryIds(yugiohCard, cardInputModel, TestData.AllCategories(), TestData.AllSubCategories());

            // Assert
            result.SubCategoryIds.Should().BeEquivalentTo(expected);
        }
    }
}
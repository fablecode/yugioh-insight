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
    public class TrapCardHelperTests
    {
        [Test]
        public void Given_A_Valid_Trap_YugiohCard_With_Types_Should_Map_To_SubCategoryIds_Property()
        {
            // Arrange
            var expected = new List<int> { 22 };

            var yugiohCard = new YugiohCard
            {
                Name = "Call of the Haunted",
                Description = "Amazing card!",
                CardNumber = "97077563",
                Property = "Continuous",
                CardType = "Trap",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/4/47/CalloftheHaunted-YS18-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180712163539"
            };
            var cardInputModel = new CardInputModel();

            // Act
            var result = TrapCardHelper.MapSubCategoryIds(yugiohCard, cardInputModel, TestData.AllCategories(), TestData.AllSubCategories());

            // Assert
            result.SubCategoryIds.Should().BeEquivalentTo(expected);
        }
    }
}
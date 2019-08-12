using cardsectionprocessor.core.Constants;
using cardsectionprocessor.core.Services;
using cardsectionprocessor.domain.Strategy;
using cardsectionprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace cardsectionprocessor.domain.unit.tests.StrategyTests.CardTipsProcessorStrategyTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private ICardTipService _cardTipService;
        private ICardService _cardService;
        private CardTipsProcessorStrategy _sut;

        [SetUp]
        public void SetUp()
        {
            _cardTipService = Substitute.For<ICardTipService>();
            _cardService = Substitute.For<ICardService>();

            _sut = new CardTipsProcessorStrategy(_cardService, _cardTipService);
        }

        [TestCase(ArticleCategory.CardTips)]
        public void Given_A_Valid_Category_Should_Return_True(string category)
        {
            // Arrange
            // Act
            var result = _sut.Handles(category);

            // Assert
            result.Should().BeTrue();
        }

        [TestCase("category")]
        public void Given_A_Valid_Category_Should_Return_False(string category)
        {
            // Arrange
            // Act
            var result = _sut.Handles(category);

            // Assert
            result.Should().BeFalse();
        }
    }
}
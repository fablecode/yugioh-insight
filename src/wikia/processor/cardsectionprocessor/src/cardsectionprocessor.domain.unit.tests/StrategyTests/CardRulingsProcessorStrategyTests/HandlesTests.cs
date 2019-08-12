using cardsectionprocessor.core.Constants;
using cardsectionprocessor.core.Services;
using cardsectionprocessor.domain.Strategy;
using cardsectionprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace cardsectionprocessor.domain.unit.tests.StrategyTests.CardRulingsProcessorStrategyTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private ICardRulingService _cardRulingService;
        private ICardService _cardService;
        private CardRulingsProcessorStrategy _sut;

        [SetUp]
        public void SetUp()
        {
            _cardRulingService = Substitute.For<ICardRulingService>();
            _cardService = Substitute.For<ICardService>();

            _sut = new CardRulingsProcessorStrategy(_cardService, _cardRulingService);
        }

        [TestCase(ArticleCategory.CardRulings)]
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
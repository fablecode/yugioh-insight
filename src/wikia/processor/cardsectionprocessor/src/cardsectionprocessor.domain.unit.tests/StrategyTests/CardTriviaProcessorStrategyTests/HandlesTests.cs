using cardsectionprocessor.core.Constants;
using cardsectionprocessor.core.Services;
using cardsectionprocessor.domain.Strategy;
using cardsectionprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace cardsectionprocessor.domain.unit.tests.StrategyTests.CardTriviaProcessorStrategyTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private ICardTriviaService _cardTriviaService;
        private ICardService _cardService;
        private CardTriviaProcessorStrategy _sut;

        [SetUp]
        public void SetUp()
        {
            _cardTriviaService = Substitute.For<ICardTriviaService>();
            _cardService = Substitute.For<ICardService>();

            _sut = new CardTriviaProcessorStrategy(_cardService, _cardTriviaService);
        }

        [TestCase(ArticleCategory.CardTrivia)]
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
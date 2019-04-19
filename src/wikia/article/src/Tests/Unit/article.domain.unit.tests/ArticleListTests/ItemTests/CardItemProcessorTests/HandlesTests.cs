using article.core.Constants;
using article.domain.ArticleList.Item;
using article.domain.Services.Messaging.Cards;
using article.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace article.domain.unit.tests.ArticleListTests.ItemTests.CardItemProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private ICardArticleQueue _cardArticleQueue;
        private CardItemProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _cardArticleQueue = Substitute.For<ICardArticleQueue>();

            _sut = new CardItemProcessor(_cardArticleQueue);
        }

        [TestCase(ArticleCategory.TcgCards)]
        [TestCase(ArticleCategory.OcgCards)]
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
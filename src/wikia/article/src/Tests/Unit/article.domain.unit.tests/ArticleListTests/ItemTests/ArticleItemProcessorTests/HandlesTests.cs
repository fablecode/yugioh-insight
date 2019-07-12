using article.core.Constants;
using article.core.Models;
using article.domain.ArticleList.Item;
using article.domain.Services.Messaging;
using article.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace article.domain.unit.tests.ArticleListTests.ItemTests.ArticleItemProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private IQueue<Article> _queue;
        private ArticleItemProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _queue = Substitute.For<IQueue<Article>>();

            _sut = new ArticleItemProcessor(_queue);
        }

        [TestCase(ArticleCategory.CardTips)]
        [TestCase(ArticleCategory.CardRulings)]
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
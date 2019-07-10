using article.core.Constants;
using article.domain.ArticleList.Item;
using article.domain.Services;
using article.domain.Services.Messaging;
using article.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace article.domain.unit.tests.ArticleListTests.ItemTests.BanlistItemProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private IBanlistArticleQueue _banlistArticleQueue;
        private BanlistItemProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _banlistArticleQueue = Substitute.For<IBanlistArticleQueue>();

            _sut = new BanlistItemProcessor(_banlistArticleQueue);
        }

        [TestCase(ArticleCategory.ForbiddenAndLimited)]
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
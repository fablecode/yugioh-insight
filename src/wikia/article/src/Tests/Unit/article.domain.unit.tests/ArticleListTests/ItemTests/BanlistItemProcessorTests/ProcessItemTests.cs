using article.core.Models;
using article.domain.ArticleList.Item;
using article.domain.Services.Messaging;
using article.tests.core;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.unit.tests.ArticleListTests.ItemTests.BanlistItemProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessItemTests
    {
        private IBanlistArticleQueue _banlistArticleQueue;
        private BanlistItemProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _banlistArticleQueue = Substitute.For<IBanlistArticleQueue>();

            _sut = new BanlistItemProcessor(_banlistArticleQueue);
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Process_Article_Successfully()
        {
            // Arrange
            var article = new UnexpandedArticle();

            // Act
            var result = await _sut.ProcessItem(article);

            // Assert
            result.IsSuccessfullyProcessed.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_Article_If_An_Exception_Is_Thrown_IsSuccessfullyProcessed_Variable_Should_False()
        {
            // Arrange
            var article = new UnexpandedArticle();
            _banlistArticleQueue.Publish(Arg.Any<UnexpandedArticle>()).Throws(new Exception());

            // Act
            var result = await _sut.ProcessItem(article);

            // Assert
            result.IsSuccessfullyProcessed.Should().BeFalse();
        }

        [Test]
        public async Task Given_A_Valid_Article_If_An_Exception_Is_Thrown_Exception_Variable_Should_Be_Initialised()
        {
            // Arrange
            var article = new UnexpandedArticle();
            _banlistArticleQueue.Publish(Arg.Any<UnexpandedArticle>()).Throws(new Exception());

            // Act
            var result = await _sut.ProcessItem(article);

            // Assert
            result.Failed.Should().NotBeNull();
        }

        [Test]
        public void Given_A_Invalid_Article_Should_Throw_NullArgument()
        {
            // Arrange

            // Act
            Func<Task<ArticleTaskResult>> act = () => _sut.ProcessItem(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
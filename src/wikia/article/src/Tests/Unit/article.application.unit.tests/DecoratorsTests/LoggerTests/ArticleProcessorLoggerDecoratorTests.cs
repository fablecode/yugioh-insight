using System;
using System.Threading.Tasks;
using article.application.Decorators.Loggers;
using article.core.ArticleList.Processor;
using article.core.Models;
using article.domain.ArticleList.Processor;
using article.tests.core;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using wikia.Models.Article.AlphabeticalList;

namespace article.application.unit.tests.DecoratorsTests.LoggerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArticleProcessorLoggerDecoratorTests
    {
        private IArticleProcessor _articleProcessor;
        private ILogger<ArticleProcessor> _logger;
        private ArticleProcessorLoggerDecorator _sut;

        [SetUp]
        public void SetUp()
        {
            _articleProcessor = Substitute.For<IArticleProcessor>();
            _logger = Substitute.For<ILogger<ArticleProcessor>>();

            _sut = new ArticleProcessorLoggerDecorator(_articleProcessor, _logger);
        }

        [Test]
        public async Task Given_A_Category_And_A_Article_Should_Invoke_LogInformation_Method_Twice()
        {
            // Arrange
            const int expected = 2;
            _articleProcessor.Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle>()).Returns(new ArticleTaskResult());

            // Act
            await _sut.Process("category", new UnexpandedArticle());

            // Assert
            _logger.Received(expected).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), Arg.Any<object>(), Arg.Any<Exception>(), Arg.Any<Func<object, Exception, string>>());
        }

        [Test]
        public void Given_A_Category_And_A_Article_If_Exception_Is_Thrown_Should_Invoke_LogError_Method_Once()
        {
            // Arrange
            const int expected = 1;
            _articleProcessor.Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle>()).Returns(new ArticleTaskResult());

            // Act
            _sut.Process("category", new UnexpandedArticle()).Throws<Exception>();

            // Assert
            _logger.Received(expected).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), Arg.Any<object>(), Arg.Any<Exception>(), Arg.Any<Func<object, Exception, string>>());
        }
    }
}
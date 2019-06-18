using System.Collections.Generic;
using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Enums;
using article.core.Models;
using article.domain.Banlist.DataSource;
using article.domain.Banlist.Processor;
using article.tests.core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistProcessorTests
    {
        private IBanlistUrlDataSource _banlistUrlDataSource;
        private IArticleProcessor _articleProcessor;
        private ILogger<BanlistProcessor> _logger;
        private BanlistProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _banlistUrlDataSource = Substitute.For<IBanlistUrlDataSource>();
            _articleProcessor = Substitute.For<IArticleProcessor>();
            _logger = Substitute.For<ILogger<BanlistProcessor>>();

            _sut = new BanlistProcessor
            (
                _banlistUrlDataSource, 
                _articleProcessor,
                _logger
            );
        }

        [Test]
        public async Task Given_A_banlistType_Should_Invoke_Process_Method_Twice()
        {
            // Arrange
            const int expected = 2;
            _banlistUrlDataSource
                .GetBanlists(Arg.Any<BanlistType>(), Arg.Any<string>())
                .Returns
                (
                    new Dictionary<int, List<int>>
                    {
                        [2017] = new List<int> {1, 2}
                    }
                );
            _articleProcessor
                .Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle>())
                .Returns(new ArticleTaskResult {IsSuccessfullyProcessed = true});

            // Act
            await _sut.Process(BanlistType.Tcg);

            // Assert
            await _articleProcessor.Received(expected).Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle>());
        }

        [Test]
        public async Task Given_A_banlistType_Should_Invoke_GetBanlists_Method_Once()
        {
            // Arrange
            const int expected = 1;
            _banlistUrlDataSource
                .GetBanlists(Arg.Any<BanlistType>(), Arg.Any<string>())
                .Returns
                (
                    new Dictionary<int, List<int>>
                    {
                        [2017] = new List<int> {1, 2}
                    }
                );
            _articleProcessor
                .Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle>())
                .Returns(new ArticleTaskResult {IsSuccessfullyProcessed = true});

            // Act
            await _sut.Process(BanlistType.Tcg);

            // Assert
            _banlistUrlDataSource.Received(expected).GetBanlists(Arg.Any<BanlistType>(), Arg.Any<string>());
        }

        [Test]
        public async Task Given_A_banlistType_Should_Return_All_Banlists()
        {
            // Arrange
            const int expected = 2;
            _banlistUrlDataSource
                .GetBanlists(Arg.Any<BanlistType>(), Arg.Any<string>())
                .Returns
                (
                    new Dictionary<int, List<int>>
                    {
                        [2017] = new List<int> {1, 2}
                    }
                );
            _articleProcessor
                .Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle>())
                .Returns(new ArticleTaskResult {IsSuccessfullyProcessed = true});

            // Act
            var result = await _sut.Process(BanlistType.Tcg);

            // Assert
            result.Processed.Should().Be(expected);
        }
    }
}
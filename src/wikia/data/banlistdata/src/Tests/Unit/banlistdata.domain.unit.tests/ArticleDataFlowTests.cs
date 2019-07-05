using System;
using System.Threading.Tasks;
using banlistdata.core.Models;
using banlistdata.core.Processor;
using banlistdata.domain.Processor;
using banlistdata.domain.Services.Messaging;
using banlistdata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace banlistdata.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArticleDataFlowTests
    {
        private IBanlistProcessor _banlistProcessor;
        private IBanlistDataQueue _banlistDataQueue;
        private ArticleDataFlow _sut;

        [SetUp]
        public void SetUp()
        {
            _banlistProcessor = Substitute.For<IBanlistProcessor>();
            _banlistDataQueue = Substitute.For<IBanlistDataQueue>();
            _sut = new ArticleDataFlow(_banlistProcessor, _banlistDataQueue);
        }

        [Test]
        public async Task Given_An_Article_Should_Invoke_Process_Once()
        {
            // Arrange
            var article = new Article { Id = 3242423, CorrelationId = Guid.NewGuid() };

            _banlistProcessor.Process(Arg.Any<Article>()).Returns(new ArticleProcessed() {Article = article, IsSuccessful = true});
            _banlistDataQueue.Publish(Arg.Any<ArticleProcessed>()).Returns(new YugiohBanlistCompletion { Article = article, IsSuccessful = true });

            // Act
            await _sut.ProcessDataFlow(article);

            // Assert
            await _banlistProcessor.Received(1).Process(Arg.Any<Article>());
        }

        [Test]
        public async Task Given_An_Article_Should_Invoke_Publish_Once()
        {
            // Arrange
            var article = new Article { Id = 3242423, CorrelationId = Guid.NewGuid() };

            _banlistProcessor.Process(Arg.Any<Article>()).Returns(new ArticleProcessed() { Article = article, IsSuccessful = true });
            _banlistDataQueue.Publish(Arg.Any<ArticleProcessed>()).Returns(new YugiohBanlistCompletion { Article = article, IsSuccessful = true});

            // Act
            await _sut.ProcessDataFlow(article);

            // Assert
            await _banlistDataQueue.Received(1).Publish(Arg.Any<ArticleProcessed>());
        }

        [Test]
        public void Given_An_Article_If_Not_Successfully_Processed_Should_Exception()
        {
            // Arrange
            var article = new Article{Id = 3242423, CorrelationId = Guid.NewGuid()};

            _banlistProcessor.Process(Arg.Any<Article>()).Returns(new ArticleProcessed {Article = article});
            _banlistDataQueue.Publish(Arg.Any<ArticleProcessed>()).Returns(new YugiohBanlistCompletion {Article = article});

            // Act
            Func<Task<ArticleCompletion>> act = () => _sut.ProcessDataFlow(article);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}
using System;
using System.Threading.Tasks;
using carddata.core.Models;
using carddata.domain.Processor;
using carddata.domain.Services.Messaging;
using carddata.domain.WebPages.Cards;
using carddata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace carddata.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArticleDataFlowTests
    {
        private IYugiohCardQueue _yugiohCardQueue;
        private ICardWebPage _cardWebPage;
        private ArticleDataFlow _sut;

        [SetUp]
        public void SetUp()
        {
            _yugiohCardQueue = Substitute.For<IYugiohCardQueue>();
            _cardWebPage = Substitute.For<ICardWebPage>();
            _sut = new ArticleDataFlow(_cardWebPage, _yugiohCardQueue);
        }

        [Test]
        public async Task Given_An_Article_Should_Invoke_GetYugiohCard_Once()
        {
            // Arrange
            var article = new Article{Id = 3242423};

            _yugiohCardQueue.Publish(Arg.Any<ArticleProcessed>()).Returns(new YugiohCardCompletion {Article = article, IsSuccessful = true});

            // Act
            await _sut.ProcessDataFlow(article);

            // Assert
            _cardWebPage.Received(1).GetYugiohCard(Arg.Is(article));
        }

        [Test]
        public async Task Given_An_Article_Should_Invoke_Publish_Once()
        {
            // Arrange
            var article = new Article{Id = 3242423};

            _yugiohCardQueue.Publish(Arg.Any<ArticleProcessed>()).Returns(new YugiohCardCompletion {Article = article, IsSuccessful = true});

            // Act
            await _sut.ProcessDataFlow(article);

            // Assert
            await _yugiohCardQueue.Received(1).Publish(Arg.Any<ArticleProcessed>());
        }

        [Test]
        public void Given_An_Article_If_Not_Successfully_Processed_Should_Exception()
        {
            // Arrange
            var article = new Article{Id = 3242423};

            _yugiohCardQueue.Publish(Arg.Any<ArticleProcessed>()).Returns(new YugiohCardCompletion {Article = article});

            // Act
            Func<Task<ArticleCompletion>> act = () => _sut.ProcessDataFlow(article);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}
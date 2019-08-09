using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using cardsectiondata.domain.ArticleList.Item;
using cardsectiondata.domain.Services.Messaging;
using cardsectiondata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cardsectiondata.domain.unit.tests.ArticleListTests.ItemTests.CardRulingItemProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessItemTests
    {
        private IEnumerable<IQueue> _queues;
        private CardRulingItemProcessor _sut;
        private ICardSectionProcessor _cardSectionProcessor;

        [SetUp]
        public void SetUp()
        {
            _queues = Substitute.For<IEnumerable<IQueue>>();

            _cardSectionProcessor = Substitute.For<ICardSectionProcessor>();
            _sut = new CardRulingItemProcessor(_cardSectionProcessor, _queues);
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Process_Article_Successfully()
        {
            // Arrange
            var article = new Article{ Title = "Call of the Haunted" };

            _cardSectionProcessor.ProcessItem(Arg.Any<Article>()).Returns(new CardSectionMessage());

            var handler = Substitute.For<IQueue>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Publish(Arg.Any<CardSectionMessage>()).Returns(Task.CompletedTask);

            _queues.GetEnumerator().Returns(new List<IQueue> {handler}.GetEnumerator());

            // Act
            var result = await _sut.ProcessItem(article);

            // Assert
            result.Article.Should().Be(article);
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Invoke_ProcessItem_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var article = new Article{ Title = "Call of the Haunted" };

            _cardSectionProcessor.ProcessItem(Arg.Any<Article>()).Returns(new CardSectionMessage());

            var handler = Substitute.For<IQueue>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Publish(Arg.Any<CardSectionMessage>()).Returns(Task.CompletedTask);

            _queues.GetEnumerator().Returns(new List<IQueue> {handler}.GetEnumerator());

            // Act
            await _sut.ProcessItem(article);

            // Assert
            await _cardSectionProcessor.Received(expected).ProcessItem(Arg.Any<Article>());
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Invoke_Publish_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var article = new Article{ Title = "Call of the Haunted" };

            _cardSectionProcessor.ProcessItem(Arg.Any<Article>()).Returns(new CardSectionMessage());

            var handler = Substitute.For<IQueue>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Publish(Arg.Any<CardSectionMessage>()).Returns(Task.CompletedTask);

            _queues.GetEnumerator().Returns(new List<IQueue> {handler}.GetEnumerator());

            // Act
            await _sut.ProcessItem(article);

            // Assert
            await handler.Received(expected).Publish(Arg.Any<CardSectionMessage>());
        }
    }
}
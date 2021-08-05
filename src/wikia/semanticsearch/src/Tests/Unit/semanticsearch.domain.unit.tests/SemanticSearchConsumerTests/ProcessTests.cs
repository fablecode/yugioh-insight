using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using semanticsearch.core.Model;
using semanticsearch.domain.Messaging.Exchanges;
using semanticsearch.domain.Search.Consumer;
using semanticsearch.tests.core;

namespace semanticsearch.domain.unit.tests.SemanticSearchConsumerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessTests
    {
        private SemanticSearchConsumer _sut;
        private IArticleHeaderExchange _articleHeaderExchange;

        [SetUp]
        public void SetUp()
        {
            _articleHeaderExchange = Substitute.For<IArticleHeaderExchange>();

            _sut = new SemanticSearchConsumer(Substitute.For<ILogger<SemanticSearchConsumer>>(), _articleHeaderExchange);
        }

        [Test]
        public async Task Given_A_SemanticCard_Should_Invoke_Publish_Once()
        {
            // Arrange
            var semanticCard = new SemanticCard();
            
            // Act
            await _sut.Process(semanticCard);

            // Assert
            await _articleHeaderExchange.Received(1).Publish(Arg.Is(semanticCard));
        }

        [Test]
        public async Task Given_A_SemanticCard_If_Process_Executed_Successful_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var semanticCard = new SemanticCard();

            // Act
            var result = await _sut.Process(semanticCard);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

    }
}

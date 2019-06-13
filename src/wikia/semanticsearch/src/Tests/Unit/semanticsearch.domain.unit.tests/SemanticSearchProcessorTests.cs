using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using semanticsearch.core.Model;
using semanticsearch.core.Search;
using semanticsearch.domain.Search;
using semanticsearch.tests.core;

namespace semanticsearch.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class SemanticSearchProcessorTests
    {
        private ISemanticSearchProducer _semanticSearchProducer;
        private ISemanticSearchConsumer _semanticSearchConsumer;
        private SemanticSearchProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _semanticSearchProducer = Substitute.For<ISemanticSearchProducer>();
            _semanticSearchConsumer = Substitute.For<ISemanticSearchConsumer>();

            _sut = new SemanticSearchProcessor(_semanticSearchProducer, _semanticSearchConsumer);
        }

        [Test]
        public async Task Given_A_Url_IsSuccessful_Should_Be_True()
        {
            // Arrange
            const string url = "https://www.youtube.com/";
            _semanticSearchConsumer.Process(Arg.Any<SemanticCard>()).Returns
            (
                new SemanticCardPublishResult
                {
                    Card = new SemanticCard(),
                    IsSuccessful = true
                },
                new SemanticCardPublishResult
                {
                    Card = new SemanticCard()
                }
            );

            // Act
            Task.Delay(2000).ContinueWith(async t =>
            {
                await _sut.CardBufferBlock.SendAsync(new SemanticCard { Title = "Test semantic card"});
                await _sut.CardBufferBlock.SendAsync(new SemanticCard { Title = "Test semantic card"});
                await Task.Delay(1000);
                _sut.CardActionBlock.Complete();
            });

            var result = await _sut.ProcessUrl(url);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
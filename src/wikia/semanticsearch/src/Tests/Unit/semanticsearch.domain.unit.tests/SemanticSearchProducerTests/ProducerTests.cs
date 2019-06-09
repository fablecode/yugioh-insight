using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using semanticsearch.core.Model;
using semanticsearch.domain.Search.Producer;
using semanticsearch.domain.WebPage;
using System;
using System.Threading.Tasks.Dataflow;

namespace semanticsearch.domain.unit.tests.SemanticSearchProducerTests
{
    [TestFixture]
    public class ProducerTests
    {
        private SemanticSearchProducer _sut;

        [SetUp]
        public void SetUp()
        {
            var semanticSearchResultsWebPage  = Substitute.For<ISemanticSearchResultsWebPage>();
            var semanticCardSearchResultsWebPage  = Substitute.For<ISemanticCardSearchResultsWebPage>();

            _sut = new SemanticSearchProducer(semanticSearchResultsWebPage, semanticCardSearchResultsWebPage);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_An_Invalid_Url_Should_Throw_ArgumentException(string url)
        {
            // Arrange

            // Act
            Action act = () => _sut.Producer(url, new BufferBlock<SemanticCard>()).Wait();

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Given_An_Invalid_TargetBlock_Should_Throw_ArgumentException()
        {
            // Arrange

            // Act
            Action act = () => _sut.Producer("https://www.youtube.com", null).Wait();

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
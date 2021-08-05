using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using semanticsearch.core.Exceptions;
using semanticsearch.core.Model;
using semanticsearch.core.Search;
using semanticsearch.domain.Search;
using semanticsearch.tests.core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

            _sut = new SemanticSearchProcessor(Substitute.For<ILogger<SemanticSearchProcessor>>(), _semanticSearchProducer, _semanticSearchConsumer);
        }

        [Test]
        public async Task Given_A_Valid_Url_Producer_Method_Should_Be_Invoked_Once()
        {
            // Arrange
            const int expected = 1;
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D";

            // Act
            var _ = await _sut.ProcessUrl(url);

            // Assert
            _semanticSearchProducer.Received(expected).Producer(Arg.Is(url));
        }

        [Test]
        public async Task Given_A_Valid_Url_Process_Method_Should_Be_Invoked_3_Times()
        {
            // Arrange
            const int expected = 3;
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D";

            _semanticSearchProducer.Producer(Arg.Is(url))
                .GetEnumerator()
                .Returns
                (
                    new List<SemanticCard>
                    {
                        new(),
                        new(),
                        new()
                    }.GetEnumerator()
                );

            _semanticSearchConsumer.Process(Arg.Any<SemanticCard>()).Returns
            (
                new SemanticCardPublishResult
                {
                    Card = new SemanticCard(),
                    IsSuccessful = true
                }
            );

            // Act
            var _ = await _sut.ProcessUrl(url);

            // Assert
            await _semanticSearchConsumer.Received(expected).Process(Arg.Any<SemanticCard>());
        }

        [Test]
        public async Task Given_A_Valid_Url_Number_Of_UnSuccessfully_Processed_Semantic_Cards_Should_Be_1()
        {
            // Arrange
            const int expected = 1;
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D";

            _semanticSearchProducer.Producer(Arg.Is(url))
                .GetEnumerator()
                .Returns
                (
                    new List<SemanticCard>
                    {
                        new(),
                        new(),
                        new()
                    }.GetEnumerator()
                );

            _semanticSearchConsumer.Process(Arg.Any<SemanticCard>()).Returns
            (
                new SemanticCardPublishResult
                {
                    Card = new SemanticCard(),
                    IsSuccessful = true
                },
                new SemanticCardPublishResult
                {
                    Card = new SemanticCard(),
                    IsSuccessful = true
                },
                new SemanticCardPublishResult
                {
                    Card = new SemanticCard(),
                    Exception = new SemanticCardPublishException { Url = url, Exception = new ArgumentNullException(nameof(url), "fail")}
                }
            );

            // Act
            var result = await _sut.ProcessUrl(url);

            // Assert
            result.Failed.Should().NotBeNullOrEmpty().And.HaveCount(expected);
        }

        [Test]
        public async Task Given_A_Valid_Url_Number_Of_Successfully_Processed_Semantic_Cards_Should_Be_2()
        {
            // Arrange
            const int expected = 2;
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D";

            _semanticSearchProducer.Producer(Arg.Is(url))
                .GetEnumerator()
                .Returns
                (
                    new List<SemanticCard>
                    {
                        new(),
                        new(),
                        new()
                    }.GetEnumerator()
                );

            _semanticSearchConsumer.Process(Arg.Any<SemanticCard>()).Returns
            (
                new SemanticCardPublishResult
                {
                    Card = new SemanticCard(),
                    IsSuccessful = true
                },
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
            var result = await _sut.ProcessUrl(url);

            // Assert
            result.Processed.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Url_IsSuccessful_Should_Be_True()
        {
            // Arrange
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D";

            // Act
            var result = await _sut.ProcessUrl(url);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
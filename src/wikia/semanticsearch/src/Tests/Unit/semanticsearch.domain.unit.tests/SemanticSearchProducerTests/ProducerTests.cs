using FluentAssertions;
using HtmlAgilityPack;
using NSubstitute;
using NUnit.Framework;
using semanticsearch.core.Model;
using semanticsearch.domain.Search.Producer;
using semanticsearch.domain.WebPage;
using semanticsearch.tests.core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace semanticsearch.domain.unit.tests.SemanticSearchProducerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProducerTests
    {
        private SemanticSearchProducer _sut;
        private ISemanticSearchResultsWebPage _semanticSearchResultsWebPage;
        private ISemanticCardSearchResultsWebPage _semanticCardSearchResultsWebPage;

        [SetUp]
        public void SetUp()
        {
            _semanticSearchResultsWebPage = Substitute.For<ISemanticSearchResultsWebPage>();
            _semanticCardSearchResultsWebPage = Substitute.For<ISemanticCardSearchResultsWebPage>();

            _sut = new SemanticSearchProducer(_semanticSearchResultsWebPage, _semanticCardSearchResultsWebPage);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_An_Invalid_Url_Should_Throw_ArgumentException(string url)
        {
            // Arrange

            // Act
            Func<IEnumerable<SemanticCard>> act = () => _sut.Producer(url).ToArray();

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Given_A_Url_With_Only_One_Page_Should_Not_Invoke_NextPageLink()
        {
            // Arrange
            var tableRows = new HtmlNodeCollection(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0))
            {
                new (HtmlNodeType.Element, new HtmlDocument(), 1)
            };

            _semanticSearchResultsWebPage.Load(Arg.Any<string>());
            _semanticSearchResultsWebPage.TableRows.Returns(tableRows);
            _semanticCardSearchResultsWebPage.Name(Arg.Any<HtmlNode>()).Returns("Card Name");
            _semanticCardSearchResultsWebPage.Url(Arg.Any<HtmlNode>(), Arg.Any<Uri>()).Returns("https://www.youtube.com");

            // Act
            _sut.Producer("https://www.youtube.com").ToArray();

            // Assert
            _semanticSearchResultsWebPage.DidNotReceive().NextPageLink();
        }

        [Test]
        public void Given_A_Url_With_2_Pages_Should_Invoke_Load_Twice()
        {
            // Arrange
            var tableRows = new HtmlNodeCollection(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0))
            {
                new (HtmlNodeType.Element, new HtmlDocument(), 1)
            };

            _semanticSearchResultsWebPage.Load(Arg.Any<string>());
            _semanticSearchResultsWebPage.TableRows.Returns(tableRows);
            _semanticSearchResultsWebPage.HasNextPage.Returns(true, true, false);
            _semanticCardSearchResultsWebPage.Name(Arg.Any<HtmlNode>()).Returns("Card Name");
            _semanticCardSearchResultsWebPage.Url(Arg.Any<HtmlNode>(), Arg.Any<Uri>()).Returns("https://www.youtube.com");

            // Act
            _sut.Producer("https://www.youtube.com").ToArray();

            // Assert
            _semanticSearchResultsWebPage.Received(2).Load(Arg.Any<string>());
        }

        [Test]
        public void Given_A_Url_With_1_Page_Should_Invoke_Load_Once()
        {
            // Arrange
            var tableRows = new HtmlNodeCollection(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0))
            {
                new (HtmlNodeType.Element, new HtmlDocument(), 1)
            };

            _semanticSearchResultsWebPage.Load(Arg.Any<string>());
            _semanticSearchResultsWebPage.TableRows.Returns(tableRows);
            _semanticSearchResultsWebPage.HasNextPage.Returns(false);
            _semanticCardSearchResultsWebPage.Name(Arg.Any<HtmlNode>()).Returns("Card Name");
            _semanticCardSearchResultsWebPage.Url(Arg.Any<HtmlNode>(), Arg.Any<Uri>()).Returns("https://www.youtube.com");

            // Act
            _sut.Producer("https://www.youtube.com").ToArray();

            // Assert
            _semanticSearchResultsWebPage.Received(1).Load(Arg.Any<string>());
        }

        [Test]
        public void Given_A_Url_With_Multiple_Pages_Should_Invoke_NextPageLink()
        {
            // Arrange
            var tableRows = new HtmlNodeCollection(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0))
            {
                new (HtmlNodeType.Element, new HtmlDocument(), 1)
            };

            _semanticSearchResultsWebPage.Load(Arg.Any<string>());
            _semanticSearchResultsWebPage.TableRows.Returns(tableRows);
            _semanticSearchResultsWebPage.HasNextPage.Returns(true, false);
            _semanticCardSearchResultsWebPage.Name(Arg.Any<HtmlNode>()).Returns("Card Name");
            _semanticCardSearchResultsWebPage.Url(Arg.Any<HtmlNode>(), Arg.Any<Uri>()).Returns("https://www.youtube.com");

            // Act
            _sut.Producer("https://www.youtube.com").ToArray();

            // Assert
            _semanticSearchResultsWebPage.Received(1).NextPageLink();
        }
    }
}
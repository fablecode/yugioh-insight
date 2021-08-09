using carddata.infrastructure.WebPages;
using carddata.infrastructure.WebPages.Cards;
using carddata.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace carddata.infrastructure.integration.tests.WebPagesTests.CardsTests.CardHtmlDocumentTests
{
    [TestFixture]
    [Category(TestType.Integration)]
    public class RankTests
    {
        private CardHtmlDocument _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CardHtmlDocument(new CardHtmlTable());
        }

        [TestCase("https://yugioh.fandom.com/wiki/Number_41:_Bagooska_the_Terribly_Tired_Tapir", 4)]
        public void Given_A_Card_Profile_WebPage_Url_Should_Extract_Card_Rank(string url, int expected)
        {
            // Arrange
            var htmlWebPage = new HtmlWebPage();
            var htmlDocument = htmlWebPage.Load(url);

            // Act
            var result = _sut.Rank(htmlDocument);

            // Assert
            result.Should().Be(expected);
        }
    }
}
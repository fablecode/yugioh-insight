using carddata.infrastructure.WebPages;
using carddata.infrastructure.WebPages.Cards;
using carddata.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace carddata.infrastructure.integration.tests.WebPagesTests.CardsTests.CardHtmlDocumentTests
{
    [TestFixture]
    [Category(TestType.Integration)]
    public class LinkArrowsTests
    {
        private CardHtmlDocument _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CardHtmlDocument(new CardHtmlTable());
        }

        [TestCase("https://yugioh.fandom.com/wiki/Firewall_Dragon", "Left , Top , Right , Bottom")]
        public void Given_A_Card_Profile_WebPage_Url_Should_Extract_Card_LinkArrows(string url, string expected)
        {
            // Arrange
            var htmlWebPage = new HtmlWebPage();
            var htmlDocument = htmlWebPage.Load(url);

            // Act
            var result = _sut.LinkArrows(htmlDocument);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
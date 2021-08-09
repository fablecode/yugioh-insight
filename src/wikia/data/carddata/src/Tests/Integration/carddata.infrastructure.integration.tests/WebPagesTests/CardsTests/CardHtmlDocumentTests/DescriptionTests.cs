using carddata.infrastructure.WebPages;
using carddata.infrastructure.WebPages.Cards;
using carddata.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace carddata.infrastructure.integration.tests.WebPagesTests.CardsTests.CardHtmlDocumentTests
{
    [TestFixture]
    [Category(TestType.Integration)]
    public class DescriptionTests
    {
        private CardHtmlDocument _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CardHtmlDocument(new CardHtmlTable());
        }

        [TestCase("https://yugioh.fandom.com/wiki/4-Starred_Ladybug_of_Doom", "FLIP: Destroy all Level 4 monsters your opponent controls.")]
        [TestCase("https://yugioh.fandom.com/wiki/Blue-Eyes_White_Dragon", "This legendary dragon is a powerful engine of destruction. Virtually invincible, very few have faced this awesome creature and lived to tell the tale.")]
        public void Given_A_Card_Profile_WebPage_Url_Should_Extract_Card_Description(string url, string expected)
        {
            // Arrange
            var htmlWebPage = new HtmlWebPage();
            var htmlDocument = htmlWebPage.Load(url);

            // Act
            var result = _sut.Description(htmlDocument);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
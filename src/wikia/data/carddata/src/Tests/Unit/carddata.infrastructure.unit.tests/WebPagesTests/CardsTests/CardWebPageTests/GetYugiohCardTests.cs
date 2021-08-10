using carddata.core.Models;
using carddata.domain.WebPages;
using carddata.domain.WebPages.Cards;
using carddata.infrastructure.WebPages.Cards;
using carddata.tests.core;
using FluentAssertions;
using HtmlAgilityPack;
using NSubstitute;
using NUnit.Framework;
using System;

namespace carddata.infrastructure.unit.tests.WebPagesTests.CardsTests.CardWebPageTests
{
    [TestFixture]
    [Category(TestType.Integration)]
    public class GetYugiohCardTests
    {
        private CardWebPage _sut;
        private ICardHtmlDocument _cardHtmlDocument;

        [SetUp]
        public void SetUp()
        {
            _cardHtmlDocument = Substitute.For<ICardHtmlDocument>();
            _sut = new CardWebPage(_cardHtmlDocument, Substitute.For<IHtmlWebPage>());
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Image()
        {
            // Arrange
            const string expected = "https://static.wikia.nocookie.net/yugioh/images/a/a1/4StarredLadybugofDoom-YSYR-EN-C-1E.png";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "4-Starred Ladybug of Doom",
                Url = "https://yugioh.fandom.com/wiki/4-Starred_Ladybug_of_Doom"
            };

            _cardHtmlDocument.ImageUrl(Arg.Any<HtmlDocument>()).Returns("https://static.wikia.nocookie.net/yugioh/images/a/a1/4StarredLadybugofDoom-YSYR-EN-C-1E.png");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.ImageUrl.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Name()
        {
            // Arrange
            const string expected = "4-Starred Ladybug of Doom";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "4-Starred Ladybug of Doom",
                Url = "https://yugioh.fandom.com/wiki/4-Starred_Ladybug_of_Doom"
            };

            _cardHtmlDocument.Name(Arg.Any<HtmlDocument>()).Returns("4-Starred Ladybug of Doom");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.Name.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Description()
        {
            // Arrange
            const string expected = "FLIP: Destroy all Level 4 monsters your opponent controls.";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "4-Starred Ladybug of Doom",
                Url = "https://yugioh.fandom.com/wiki/4-Starred_Ladybug_of_Doom"
            };

            _cardHtmlDocument.Description(Arg.Any<HtmlDocument>()).Returns("FLIP: Destroy all Level 4 monsters your opponent controls.");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.Description.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Number()
        {
            // Arrange
            const string expected = "83994646";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "4-Starred Ladybug of Doom",
                Url = "https://yugioh.fandom.com/wiki/4-Starred_Ladybug_of_Doom"
            };

            _cardHtmlDocument.CardNumber(Arg.Any<HtmlDocument>()).Returns("83994646");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.CardNumber.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Type()
        {
            // Arrange
            const string expected = "Monster";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "4-Starred Ladybug of Doom",
                Url = "https://yugioh.fandom.com/wiki/4-Starred_Ladybug_of_Doom"
            };

            _cardHtmlDocument.CardType(Arg.Any<HtmlDocument>()).Returns("Monster");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.CardType.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Attribute()
        {
            // Arrange
            const string expected = "Wind";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "4-Starred Ladybug of Doom",
                Url = "https://yugioh.fandom.com/wiki/4-Starred_Ladybug_of_Doom"
            };

            _cardHtmlDocument.Attribute(Arg.Any<HtmlDocument>()).Returns("Wind");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.Attribute.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Rank()
        {
            // Arrange
            const int expected = 4;
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "Bagooska the Terribly Tired Tapir",
                Url = "https://yugioh.fandom.com/wiki/Number_41:_Bagooska_the_Terribly_Tired_Tapir"
            };

            _cardHtmlDocument.Rank(Arg.Any<HtmlDocument>()).Returns(4);

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.Rank.Should().Be(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_AtkDef()
        {
            // Arrange
            const string expected = "2100 / 2000";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "Bagooska the Terribly Tired Tapir",
                Url = "https://yugioh.fandom.com/wiki/Number_41:_Bagooska_the_Terribly_Tired_Tapir"
            };

            _cardHtmlDocument.AtkDef(Arg.Any<HtmlDocument>()).Returns("2100 / 2000");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.AtkDef.Should().Be(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_AtkLink()
        {
            // Arrange
            const string expected = "2500 / 4";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "Firewall Dragon",
                Url = "https://yugioh.fandom.com/wiki/Firewall_Dragon"
            };

            _cardHtmlDocument.AtkLink(Arg.Any<HtmlDocument>()).Returns("2500 / 4");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.AtkLink.Should().Be(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Types()
        {
            // Arrange
            const string expected = "Cyberse / Link / Effect";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "Firewall Dragon",
                Url = "https://yugioh.fandom.com/wiki/Firewall_Dragon"
            };

            _cardHtmlDocument.Types(Arg.Any<HtmlDocument>()).Returns("Cyberse / Link / Effect");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.Types.Should().Be(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Materials()
        {
            // Arrange
            const string expected = "2+ monsters";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "Firewall Dragon",
                Url = "https://yugioh.fandom.com/wiki/Firewall_Dragon"
            };

            _cardHtmlDocument.Materials(Arg.Any<HtmlDocument>()).Returns("2+ monsters");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.Materials.Should().Be(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_CardEffectTypes()
        {
            // Arrange
            const string expected = "Quick,Trigger";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "Firewall Dragon",
                Url = "https://yugioh.fandom.com/wiki/Firewall_Dragon"
            };

            _cardHtmlDocument.CardEffectTypes(Arg.Any<HtmlDocument>()).Returns("Quick,Trigger");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.CardEffectTypes.Should().Be(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_PendulumScale()
        {
            // Arrange
            const int expected =7;
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "Archfiend Eccentrick",
                Url = "https://yugioh.fandom.com/wiki/Archfiend_Eccentrick"
            };

            _cardHtmlDocument.PendulumScale(Arg.Any<HtmlDocument>()).Returns(7);

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.PendulumScale.Should().Be(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Level()
        {
            // Arrange
            const int expected = 4;
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "4-Starred Ladybug of Doom",
                Url = "https://yugioh.fandom.com/wiki/4-Starred_Ladybug_of_Doom"
            };

            _cardHtmlDocument.Level(Arg.Any<HtmlDocument>()).Returns(4);

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.Level.Should().Be(expected);
        }

        [Test]
        public void Given_An_Card_Article_Should_Extract_Card_Property()
        {
            // Arrange
            const string expected = "Normal";
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Title = "Monster Reborn",
                Url = "https://yugioh.fandom.com/wiki/Monster_Reborn"
            };

            _cardHtmlDocument.Property(Arg.Any<HtmlDocument>()).Returns("Normal");

            // Act
            var result = _sut.GetYugiohCard(article);

            // Assert
            result.Card.Property.Should().BeEquivalentTo(expected);
        }
    }
}
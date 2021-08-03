using System;
using archetypedata.application.Configuration;
using archetypedata.domain.WebPages;
using archetypedata.infrastructure.WebPages;
using archetypedata.tests.core;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace archetypedata.infrastructure.integration.tests.WebPagesTests.ArchetypeWebPageTests
{
    [TestFixture]
    [Category(TestType.Integration)]
    public class CardsTests
    {
        private ArchetypeWebPage _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ArchetypeWebPage(Substitute.For<IOptions<AppSettings>>(), new HtmlWebPage(), Substitute.For<IArchetypeThumbnail>());
        }

        [TestCase("https://yugioh.fandom.com/wiki/List_of_%22/Assault_Mode%22_cards", new [] {"Stardust Dragon/Assault Mode", "Colossal Fighter/Assault Mode", "Arcanite Magician/Assault Mode" })]
        [TestCase("https://yugioh.fandom.com/wiki/List_of_%22Elemental_HERO%22_cards", new [] { "Contrast HERO Chaos", "Elemental HERO Plasma Vice", "Elemental HERO Voltic" })]
        public void Given_An_Archetype_Web_Page_Uri_Should_Extract_Card_Names_From_Web_Page(string archetypeUrl, string[] expected)
        {
            // Arrange
            var archetypeUri = new Uri(archetypeUrl);

            // Act
            var result = _sut.Cards(archetypeUri);

            // Assert
            result.Should().Contain(expected);
        }
    }
}
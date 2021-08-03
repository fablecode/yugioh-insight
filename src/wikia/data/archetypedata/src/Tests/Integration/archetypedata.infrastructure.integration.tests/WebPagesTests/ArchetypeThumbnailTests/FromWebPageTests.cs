using archetypedata.application.Configuration;
using archetypedata.infrastructure.WebPages;
using archetypedata.tests.core;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using wikia.Api;

namespace archetypedata.infrastructure.integration.tests.WebPagesTests.ArchetypeThumbnailTests
{
    [TestFixture]
    [Category(TestType.Integration)]
    public class FromWebPageTests
    {
        private ArchetypeThumbnail _sut;
        private IOptions<AppSettings> _appsettings;

        [SetUp]
        public void SetUp()
        {
            _appsettings = Substitute.For<IOptions<AppSettings>>();
            _sut = new ArchetypeThumbnail(Substitute.For<IWikiArticle>(), new HtmlWebPage(), _appsettings);
        }

        [TestCase("/wiki/Metaphys", "https://static.wikia.nocookie.net/yugioh/images/0/0a/Metaphys.png")]
        public void Given_An_Archetype_Web_Page_Url_Should_Return_Thumbnail_Url(string archetypeUrl, string expected)
        {
            // Arrange
            _appsettings.Value.Returns(new AppSettings {WikiaDomainUrl = "https://yugioh.fandom.com"});

            // Act
            var result = _sut.FromWebPage(archetypeUrl);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
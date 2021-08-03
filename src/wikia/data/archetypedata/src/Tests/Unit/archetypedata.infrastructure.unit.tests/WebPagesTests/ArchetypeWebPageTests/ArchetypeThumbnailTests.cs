using System.Collections.Generic;
using System.Threading.Tasks;
using archetypedata.application.Configuration;
using archetypedata.domain.WebPages;
using archetypedata.infrastructure.WebPages;
using archetypedata.tests.core;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using wikia.Models.Article.Details;

namespace archetypedata.infrastructure.unit.tests.WebPagesTests.ArchetypeWebPageTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeThumbnailTests
    {
        private ArchetypeWebPage _sut;
        private IHtmlWebPage _htmlWebPage;
        private IArchetypeThumbnail _archetypeThumbnail;

        [SetUp]
        public void SetUp()
        {
            _htmlWebPage = Substitute.For<IHtmlWebPage>();
            _archetypeThumbnail = Substitute.For<IArchetypeThumbnail>();
            _sut = new ArchetypeWebPage(Substitute.For<IOptions<AppSettings>>(), _htmlWebPage, _archetypeThumbnail);
        }

        [Test]
        public void Given_A_Thumbnail_Url_And_A_Archetype_Web_Page_Url_If_Thumbnail_Url_Is_NullOrEmpty_Should_Invoke_ArchetypeThumbnailFromWebPage_Method_Once()
        {
            // Arrange
            const string articleUrl = "https://yugioh.fandom.com/wiki/Tentacluster";

            // Act
            _sut.ArchetypeThumbnail(string.Empty, articleUrl);

            // Assert
            _archetypeThumbnail.Received(1).FromWebPage(Arg.Is(articleUrl));
        }
        [Test]
        public void Given_A_Thumbnail_Url_And_A_Archetype_Web_Page_Url_If_Thumbnail_Url_Is_Not_NullOrEmpty_Should_Return_Thumbnail_Url()
        {
            // Arrange
            const string thumbnailUrl = "https://static.wikia.nocookie.net/yugioh/images/7/7c/Tentacluster.png";
            const string articleUrl = "https://yugioh.fandom.com/wiki/Tentacluster";

            // Act
            var result = _sut.ArchetypeThumbnail(thumbnailUrl, articleUrl);

            // Assert
            result.Should().BeEquivalentTo(thumbnailUrl);
        }

        [Test]
        public void Given_ArticleDetails_And_A_Archetype_Url_Should_Invoke_ArchetypeType_FromArticleDetails_Method_Once()
        {
            // Arrange
            var articleDetails = new KeyValuePair<string, ExpandedArticle>("666747", new ExpandedArticle
            {
                Thumbnail = "https://static.wikia.nocookie.net/yugioh/images/7/7c/Tentacluster.png/revision/latest/smart/width/200/height/200?cb=20170830132740"
            });
            const string archetypeWebPageUrl = "https://yugioh.fandom.com/wiki/Tentacluster";

            // Act
            _sut.ArchetypeThumbnail(articleDetails, archetypeWebPageUrl);

            // Assert
            _archetypeThumbnail.Received(1).FromArticleDetails(Arg.Is(articleDetails));
        }

        [Test]
        public async Task Given_An_ArticleId_And_A_Archetype_Url_Should_Invoke_ArchetypeType_FromArticleId_Method_Once()
        {
            // Arrange
            const int articleId = 666747;
            const string archetypeWebPageUrl = "https://yugioh.fandom.com/wiki/Tentacluster";

            // Act
            await _sut.ArchetypeThumbnail(articleId, archetypeWebPageUrl);

            // Assert
            await _archetypeThumbnail.Received(1).FromArticleId(Arg.Is(articleId));
        }
    }
}
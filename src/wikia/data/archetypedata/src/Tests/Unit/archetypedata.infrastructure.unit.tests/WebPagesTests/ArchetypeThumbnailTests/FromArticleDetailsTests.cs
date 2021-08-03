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
using wikia.Api;
using wikia.Models.Article.Details;

namespace archetypedata.infrastructure.unit.tests.WebPagesTests.ArchetypeThumbnailTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class FromArticleDetailsTests
    {
        private ArchetypeThumbnail _sut;
        private IWikiArticle _wikiArticle;

        [SetUp]
        public void SetUp()
        {
            _wikiArticle = Substitute.For<IWikiArticle>();
            _sut = new ArchetypeThumbnail(_wikiArticle, Substitute.For<IHtmlWebPage>(), Substitute.For<IOptions<AppSettings>>());
        }

        [Test]
        public void Given_ArticleDetails_Should_Extract_Article_Thumbnail()
        {
            // Arrange
            const string expected = "https://static.wikia.nocookie.net/yugioh/images/7/7c/Tentacluster.png";

            var articleDetails = new KeyValuePair<string, ExpandedArticle>("666747", new ExpandedArticle
            {
                Thumbnail = "https://static.wikia.nocookie.net/yugioh/images/7/7c/Tentacluster.png/revision/latest/smart/width/200/height/200?cb=20170830132740"
            });

            // Act
            var result = _sut.FromArticleDetails(articleDetails);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_An_Article_Id_Should_Invoke_Wikia_Details_Method_Once()
        {
            // Arrange
            const int expected = 1;
            const int articleId = 666747;

            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet
            {
                Items = new Dictionary<string, ExpandedArticle>
                {
                    ["666747"] = new()
                    {
                        Thumbnail =
                            "https://static.wikia.nocookie.net/yugioh/images/7/7c/Tentacluster.png/revision/latest/smart/width/200/height/200?cb=20170830132740"
                    }
                }
            });

            // Act
            await _sut.FromArticleId(articleId);

            // Assert
            await _wikiArticle.Received(expected).Details(Arg.Is(articleId));
        }
    }
}
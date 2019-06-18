using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using article.core.Enums;
using article.domain.Banlist.DataSource;
using article.domain.WebPages;
using article.domain.WebPages.Banlists;
using article.tests.core;
using FluentAssertions;
using HtmlAgilityPack;
using NSubstitute;
using NUnit.Framework;

namespace article.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistUrlDataSourceTests
    {
        private IBanlistWebPage _banlistWebPage;
        private IHtmlWebPage _htmlWebPage;
        private BanlistUrlDataSource _sut;

        [SetUp]
        public void SetUp()
        {
            _banlistWebPage = Substitute.For<IBanlistWebPage>();
            _htmlWebPage = Substitute.For<IHtmlWebPage>();

            _sut = new BanlistUrlDataSource(_banlistWebPage, _htmlWebPage);
        }

        [Test]
        public void Given_BanlistType_And_A_BanlistUrl_Should_Return_All_Banlists_GroupedBy_Year()
        {
            // Arrange
            var banlistType = BanlistType.Tcg;
            var banlistUrl = "http://www.youtube.com";

            _banlistWebPage
                .GetBanlistUrlList(Arg.Any<BanlistType>(), Arg.Any<string>())
                .Returns(new Dictionary<string, List<Uri>>
                {
                    ["2017"] = new List<Uri> { new Uri("http://www.youtube.com") }
                });

            var htmlDocument = new HtmlDocument();

            htmlDocument.DocumentNode.InnerHtml = "\"<script>wgArticleId=296,</script>\"";

            _htmlWebPage.Load(Arg.Any<Uri>()).Returns(htmlDocument);

            // Act
            var result = _sut.GetBanlists(banlistType, banlistUrl);

            // Assert
            result.Should().NotBeEmpty();
        }

        [Test]
        public void Given_BanlistType_And_A_BanlistUrl_Should_Invoke_GetBanlistUrlList_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var banlistType = BanlistType.Tcg;
            var banlistUrl = "http://www.youtube.com";

            _banlistWebPage
                .GetBanlistUrlList(Arg.Any<BanlistType>(), Arg.Any<string>())
                .Returns(new Dictionary<string, List<Uri>>
                {
                    ["2017"] = new List<Uri> { new Uri("http://www.youtube.com") }
                });

            var htmlDocument = new HtmlDocument();

            htmlDocument.DocumentNode.InnerHtml = "\"<script>wgArticleId=296,</script>\"";

            _htmlWebPage.Load(Arg.Any<Uri>()).Returns(htmlDocument);

            // Act
            _sut.GetBanlists(banlistType, banlistUrl);

            // Assert
            _banlistWebPage.Received(expected).GetBanlistUrlList(Arg.Any<BanlistType>(), Arg.Any<string>());
        }


        [Test]
        public void Given_BanlistType_And_A_BanlistUrl_Should_Invoke_Load_Method_Once()
        {
            // Arrange
            const int expected = 2;
            var banlistType = BanlistType.Tcg;
            var banlistUrl = "http://www.youtube.com";

            _banlistWebPage
                .GetBanlistUrlList(Arg.Any<BanlistType>(), Arg.Any<string>())
                .Returns(new Dictionary<string, List<Uri>>
                {
                    ["2017"] = new List<Uri> { new Uri("http://www.youtube.com") },
                    ["2018"] = new List<Uri> { new Uri("http://www.youtube.com") }
                });

            var htmlDocument = new HtmlDocument();

            htmlDocument.DocumentNode.InnerHtml = "\"<script>wgArticleId=296,</script>\"";

            _htmlWebPage.Load(Arg.Any<Uri>()).Returns(htmlDocument);

            // Act
            _sut.GetBanlists(banlistType, banlistUrl);

            // Assert
            _htmlWebPage.Received(expected).Load(Arg.Any<Uri>());
        }
    }
}
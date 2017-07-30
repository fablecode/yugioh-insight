using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using wikia.Models.Article.Simple;
using yugioh_insight.Domain;
using yugioh_insight.Domain.Client;
using yugioh_insight.Models.BanlistModels;

namespace yugioh_insight.unit.tests.BanlistTests
{
    [TestFixture]
    public class LatestBanlistTests
    {
        private IBanlistManager _banlistManager;
        private IYugiohClient _yugiohClient;

        [SetUp]
        public void SetUp()
        {
            _yugiohClient = Substitute.For<IYugiohClient>();

            _banlistManager = new BanlistManager(_yugiohClient);
        }

        [Test]
        public void Given_A_Null_BanlistArticleSummary_Should_Throw_ArgumentNullException()
        {
            // Arrange
            // Act
            Func<Task<Banlist>> act = () => _banlistManager.LatestBanlist(null);

            // Assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Given_A_BanlistArticleSummary_With_An_Invalid_ArticleId_Should_Throw_ArgumentException()
        {
            // Arrange
            // Act
            Func<Task<Banlist>> act = () => _banlistManager.LatestBanlist(new BanlistArticleSummary());

            // Assert
            act.ShouldThrow<ArgumentException>();
        }

        [TestCase("Banlist.LatestTcgBanlist.json")]
        [TestCase("Banlist.LatestOcgBanlist.json")]
        public async Task Given_A_BanlistArticleSummary_Should_Return_Banlist(string jsonResourceName)
        {
            // Arrange
            var json = GetFileContents(jsonResourceName);
            var contentResult = JsonConvert.DeserializeObject<ContentResult>(json);
            _yugiohClient.ArticleSimple(Arg.Any<int>()).Returns(contentResult);

            // Act
            var result = await _banlistManager.LatestBanlist(new BanlistArticleSummary { ArticleId = 324242 });

            // Assert
            result.Should().NotBeNull();
        }


        [TestCase("Banlist.LatestTcgBanlist.json")]
        [TestCase("Banlist.LatestOcgBanlist.json")]
        public async Task Given_A_BanlistArticleSummary_Should_Return_Banlist_Sections(string jsonResourceName)
        {
            // Arrange
            var json = GetFileContents(jsonResourceName);
            var contentResult = JsonConvert.DeserializeObject<ContentResult>(json);
            _yugiohClient.ArticleSimple(Arg.Any<int>()).Returns(contentResult);

            // Act
            var result = await _banlistManager.LatestBanlist(new BanlistArticleSummary{ArticleId = 324242});

            // Assert
            result.Sections.Should().NotBeEmpty();
        }


        private string GetFileContents(string sampleFile)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resource = $"yugioh_insight.unit.tests.Test_Data.{sampleFile}";
            using (var stream = asm.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }

    }

}
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using yugioh_insight.Domain;
using yugioh_insight.Enums;

namespace yugioh_insight.integration.tests.BanlistTests
{
    [TestFixture]
    public class LatestBanlistTests
    {
        private BanlistManager _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new BanlistManager();
        }

        [TestCase(BanlistType.Tcg)]
        [TestCase(BanlistType.Ocg)]
        public async Task Given_A_BanlistType_Should_Return_Latest_banlist(BanlistType banlistType)
        {
            // Arrange
            // Act
            var result = await _sut.LatestBanlist(banlistType);

            // Assert
            result.Should().NotBeNull();
        }

        [TestCase(BanlistType.Tcg)]
        [TestCase(BanlistType.Ocg)]
        public async Task Given_A_BanlistType_Banlist_Sections_Should_Not_Be_Empty(BanlistType banlistType)
        {
            // Arrange
            // Act
            var result = await _sut.LatestBanlist(banlistType);

            // Assert
            result.Sections.Should().NotBeEmpty();
        }

    }
}
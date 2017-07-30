using System;
using FluentAssertions;
using NUnit.Framework;
using yugioh_insight.Domain;
using yugioh_insight.Enums;
using yugioh_insight.Helpers;

namespace yugioh_insight.unit.tests.BanlistTests
{
    [TestFixture]
    public class ExtractBanlistArticleSummaryTests
    {
        [TestCase("OCG in effect since September 1, 2007.", BanlistType.Ocg)]
        [TestCase("TCG in effect since January 1, 2016.", BanlistType.Tcg)]
        [TestCase("TCG in effect since April 1, 2010", BanlistType.Tcg)]
        public void Given_A_Banlist_TitleText_Should_Extract_BanlistType(string titleText, BanlistType expected)
        {
            // Arrange

            // Act
            var result = BanlistHelpers.ExtractBanlistArticleDetails(titleText);

            // Assert
            result.BanlistType.Should().Be(expected);
        }

        [TestCase("OCG in effect since September 1, 2007.", "September 1, 2007")]
        [TestCase("TCG in effect since January 1, 2016.", "January 1, 2016")]
        [TestCase("TCG in effect since April 1, 2010", "April 1, 2010")]
        public void Given_A_Banlist_TitleText_Should_Extract_Banlist_StartDate(string titleText, DateTime expected)
        {
            // Arrange

            // Act
            var result = BanlistHelpers.ExtractBanlistArticleDetails(titleText);

            // Assert
            result.StartDate.Should().Be(expected);
        }

    }
}
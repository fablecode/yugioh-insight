using FluentAssertions;
using NUnit.Framework;
using yugioh_insight.Domain;
using yugioh_insight.Helpers;

namespace yugioh_insight.unit.tests
{
    [TestFixture]
    public class StringHelpersTests
    {
        [TestCase(@"Chaos Emperor Dragon - Envoy of the End 「 －の－」", "Chaos Emperor Dragon - Envoy of the End")]
        [TestCase(@"Dark Strike Fighter 「ダーク・ダイブ・ボンバー」", "Dark Strike Fighter")]
        [TestCase(@"Mind Master 「メンタルマスター」", "Mind Master")]
        public void Given_A_Text_Should_Remove_Content_Between_Begin_Char_And_End_Char(string text, string expected)
        {
            // Arrange
            var beginChar = '「';
            var endChar = '」';

            // Act
            var result = StringHelpers.RemoveBetween(text, beginChar, endChar);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
using System;
using banlistdata.domain.Helpers;
using banlistdata.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace banlistdata.domain.unit.tests.HelpersTests.StringHelpersTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class RemoveBetweenTests
    {
        [TestCase("Fishborg Blaster 「フィッシュボーグ－ガンナー」", "Fishborg Blaster")]
        [TestCase("Majespecter Unicorn - Kirin 「マジェスペクター・ユニコーン」", "Majespecter Unicorn - Kirin")]
        [TestCase("The Tyrant Neptune 「Ｔｈｅ　ｔｙｒａｎｔ　ＮＥＰＴＵＮＥザ・タイラント・ネプチューン」", "The Tyrant Neptune")]
        public void Given_A_CardName_With_Japanese_Characters_Should_Remove_Invalid_Characters(string cardName, string expected)
        {
            // Arrange
            const char beginChar = '「';
            const char endChar = '」';

            // Act
            var result = StringHelpers.RemoveBetween(cardName, beginChar, endChar);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("List of \"Fire King\" cards", "Fire King")]
        [TestCase("List of \"Gravekeeper's\" cards", "Gravekeeper's")]
        [TestCase("List of \"/Assault Mode\" cards", "/Assault Mode")]
        public void Given_An_Archetype_ListTitle_Should_Return_Archetype_Name(string archetypeListTitle, string expected)
        {
            // Arrange
            // Act
            var result = StringHelpers.ArchetypeNameFromListTitle(archetypeListTitle);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_An_Invalid_Archetype_ListTitle_Should_Throw_ArgumentNullException()
        {
            // Arrange
            // Act
            Action act = () => StringHelpers.ArchetypeNameFromListTitle(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Given_An_Invalid_Archetype_ListTitle_Should_Return_Null()
        {
            // Arrange
            // Act
            var result = StringHelpers.ArchetypeNameFromListTitle("title");

            // Assert
            result.Should().BeNull();
        }
    }
}
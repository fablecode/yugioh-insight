using cardsectiondata.domain.Helpers;
using cardsectiondata.tests.core;
using FluentAssertions;
using NUnit.Framework;
using wikia.Models.Article.Simple;

namespace cardsectiondata.domain.unit.tests.HelpersTests.SectionHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class GetSectionContentListTests
    {
        [TestCaseSource(nameof(_getSectionContentListCases))]
        public void Given_A_Section_With_ContentList_Should_Return_All_ListElements(Section section, string[] expected)
        {
            // Assert

            // Act
            var result = SectionHelper.GetSectionContentList(section);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        static object[] _getSectionContentListCases =
        {
            new object[] {new Section
            {
                Content = new[]
                {
                    new SectionContent
                    {
                        Elements = new[]
                        {
                            new ListElement
                            {
                                Text = "Ancient Fairy Dragon"
                            },
                            new ListElement
                            {
                                Text = "Blaster, Dragon Ruler of Infernos"
                            },
                            new ListElement
                            {
                                Text = "Cyber Jar"
                            }
                        }
                    }
                }
            }, new [] {"Cyber Jar", "Ancient Fairy Dragon", "Blaster, Dragon Ruler of Infernos"}}
        };

    }
}
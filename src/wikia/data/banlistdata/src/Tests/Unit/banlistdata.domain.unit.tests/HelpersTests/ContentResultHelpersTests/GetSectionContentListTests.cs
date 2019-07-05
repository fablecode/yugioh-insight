using banlistdata.domain.Helpers;
using banlistdata.tests.core;
using FluentAssertions;
using NUnit.Framework;
using wikia.Models.Article.Simple;

namespace banlistdata.domain.unit.tests.HelpersTests.ContentResultHelpersTests
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
            var result = ContentResultHelpers.GetSectionContentList(section);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(nameof(_getContentListCases))]
        public void Given_A_Section_With_ContentList_Should_Use_Recursion_To_Traverse_Data(Section section, string[] expected)
        {
            // Assert

            // Act
            var result = ContentResultHelpers.GetSectionContentList(section);

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

        static object[] _getContentListCases =
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
                                Text = "Blaster, Dragon Ruler of Infernos",
                                Elements = new[]
                                {
                                    new ListElement
                                    {
                                        Text = "Master Peace, the True Dracoslaying King",
                                        Elements = new[]
                                        {
                                            new ListElement
                                            {
                                                Text = "Mind Master"
                                            }
                                        }
                                    }
                                }
                            },
                            new ListElement
                            {
                                Text = "Cyber Jar"
                            }
                        }
                    }
                }
            }, new [] {"Cyber Jar", "Ancient Fairy Dragon", "Blaster, Dragon Ruler of Infernos", "Master Peace, the True Dracoslaying King", "Mind Master"}}
        };
    }
}
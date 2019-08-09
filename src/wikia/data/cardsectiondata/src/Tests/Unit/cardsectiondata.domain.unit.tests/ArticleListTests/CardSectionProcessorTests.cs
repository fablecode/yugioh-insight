using System.Threading.Tasks;
using cardsectiondata.core.Models;
using cardsectiondata.domain.ArticleList.Processor;
using cardsectiondata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using wikia.Api;
using wikia.Models.Article.Simple;

namespace cardsectiondata.domain.unit.tests.ArticleListTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardSectionProcessorTests
    {
        private IWikiArticle _wikiArticle;
        private CardSectionProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _wikiArticle = Substitute.For<IWikiArticle>();

            _sut = new CardSectionProcessor(_wikiArticle);
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Return_CardSectionMessage_With_Correct_Title()
        {
            // Arrange
            const string expected = "Call Of The Haunted";

            var article = new Article{ Title = "Call Of The Haunted" };

            _wikiArticle.Simple(Arg.Any<long>()).Returns(new ContentResult
            {
                Sections = new[]
                {
                    new Section
                    {
                        Title = "Call of the Haunted",
                        Level = 1,
                        Content = new[]
                        {
                            new SectionContent
                            {
                                Type = "list",
                                Elements = new[]
                                {
                                    new ListElement
                                    {
                                        Text = "This card can be searched by \"A Cat of Ill Omen\" and \"The Despair Uranus\"."
                                    },
                                }
                            },
                        }
                    }
                }
            });

            // Act
            var result = await _sut.ProcessItem(article);

            // Assert
            result.Name.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Return_CardSectionMessage_ContentList_Containing_1_Item()
        {
            // Arrange
            const int expected = 1;

            var article = new Article{ Title = "Call Of The Haunted" };

            _wikiArticle.Simple(Arg.Any<long>()).Returns(new ContentResult
            {
                Sections = new[]
                {
                    new Section
                    {
                        Title = "Call of the Haunted",
                        Level = 1,
                        Content = new[]
                        {
                            new SectionContent
                            {
                                Type = "list",
                                Elements = new[]
                                {
                                    new ListElement
                                    {
                                        Text = "This card can be searched by \"A Cat of Ill Omen\" and \"The Despair Uranus\"."
                                    },
                                }
                            },
                        }
                    }
                }
            });

            // Act
            var result = await _sut.ProcessItem(article);

            // Assert
            result.CardSections.Count.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_Article_If_It_Contains_A_Section_With_The_Title_References_It_Should_Not_Be_Processed()
        {
            // Arrange
            const int expected = 1;

            var article = new Article{ Title = "Call Of The Haunted" };

            _wikiArticle.Simple(Arg.Any<long>()).Returns(new ContentResult
            {
                Sections = new[]
                {
                    new Section
                    {
                        Title = "Call of the Haunted",
                        Level = 1,
                        Content = new[]
                        {
                            new SectionContent
                            {
                                Type = "list",
                                Elements = new[]
                                {
                                    new ListElement
                                    {
                                        Text = "This card can be searched by \"A Cat of Ill Omen\" and \"The Despair Uranus\"."
                                    },
                                }
                            },
                        }
                    },
                    new Section
                    {
                        Title = "References"
                    }, 
                }
            });

            // Act
            var result = await _sut.ProcessItem(article);

            // Assert
            result.CardSections.Count.Should().Be(expected);
        }
    }
}
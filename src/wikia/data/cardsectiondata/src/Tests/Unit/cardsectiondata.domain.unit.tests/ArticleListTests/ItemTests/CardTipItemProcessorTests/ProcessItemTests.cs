using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectiondata.core.Models;
using cardsectiondata.domain.ArticleList.Item;
using cardsectiondata.domain.Services.Messaging;
using cardsectiondata.domain.WebPages;
using cardsectiondata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using wikia.Api;
using wikia.Models.Article.Simple;

namespace cardsectiondata.domain.unit.tests.ArticleListTests.ItemTests.CardTipItemProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessItemTests
    {
        private IWikiArticle _wikiArticle;
        private ITipRelatedWebPage _tipRelatedWebPage;
        private IEnumerable<IQueue> _queues;
        private CardTipItemProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _wikiArticle = Substitute.For<IWikiArticle>();
            _tipRelatedWebPage = Substitute.For<ITipRelatedWebPage>();
            _queues = Substitute.For<IEnumerable<IQueue>>();

            _sut = new CardTipItemProcessor(_wikiArticle, _tipRelatedWebPage, _queues);
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Process_Article_Successfully()
        {
            // Arrange
            var article = new Article{ Title = "Call of the Haunted" };

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
                                        Text = "This card can be searched by \"A Cat of Ill Omen\" and \"The Despair Uranus\".",
                                        Elements = new ListElement[0]
                                    },
                                }
                            },
                        }
                    },
                    new Section
                    {
                        Title = "References",
                        Content = new SectionContent[0]
                    },
                    new Section
                    {
                        Title = "List",
                        Content = new []
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
                    },
                }
            });

            var handler = Substitute.For<IQueue>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Publish(Arg.Any<CardSectionMessage>()).Returns(Task.CompletedTask);

            _queues.GetEnumerator().Returns(new List<IQueue> {handler}.GetEnumerator());

            // Act
            var result = await _sut.ProcessItem(article);

            // Assert
            result.Article.Should().Be(article);
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Invoke_Simple_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var article = new Article{ Title = "Call of the Haunted" };

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
                                        Text = "This card can be searched by \"A Cat of Ill Omen\" and \"The Despair Uranus\".",
                                        Elements = new ListElement[0]
                                    },
                                }
                            },
                        }
                    },
                    new Section
                    {
                        Title = "References",
                        Content = new SectionContent[0]
                    },
                    new Section
                    {
                        Title = "List",
                        Content = new []
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
                    },
                }
            });

            var handler = Substitute.For<IQueue>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Publish(Arg.Any<CardSectionMessage>()).Returns(Task.CompletedTask);

            _queues.GetEnumerator().Returns(new List<IQueue> {handler}.GetEnumerator());

            // Act
            await _sut.ProcessItem(article);

            // Assert
            await _wikiArticle.Received(expected).Simple(Arg.Any<long>());
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Invoke_GetTipRelatedCards_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var article = new Article{ Title = "Call of the Haunted" };

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
                                        Text = "This card can be searched by \"A Cat of Ill Omen\" and \"The Despair Uranus\".",
                                        Elements = new ListElement[0]
                                    },
                                }
                            },
                        }
                    },
                    new Section
                    {
                        Title = "References",
                        Content = new SectionContent[0]
                    },
                    new Section
                    {
                        Title = "List",
                        Content = new []
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
                    },
                }
            });

            var handler = Substitute.For<IQueue>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Publish(Arg.Any<CardSectionMessage>()).Returns(Task.CompletedTask);

            _queues.GetEnumerator().Returns(new List<IQueue> {handler}.GetEnumerator());

            // Act
            await _sut.ProcessItem(article);

            // Assert
            _tipRelatedWebPage.Received(expected).GetTipRelatedCards(Arg.Any<CardSection>(), Arg.Any<Article>());
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Invoke_Publish_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var article = new Article{ Title = "Call of the Haunted" };

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
                                        Text = "This card can be searched by \"A Cat of Ill Omen\" and \"The Despair Uranus\".",
                                        Elements = new ListElement[0]
                                    },
                                }
                            },
                        }
                    },
                    new Section
                    {
                        Title = "References",
                        Content = new SectionContent[0]
                    },
                    new Section
                    {
                        Title = "List",
                        Content = new []
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
                    },
                }
            });

            var handler = Substitute.For<IQueue>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Publish(Arg.Any<CardSectionMessage>()).Returns(Task.CompletedTask);

            _queues.GetEnumerator().Returns(new List<IQueue> {handler}.GetEnumerator());

            // Act
            await _sut.ProcessItem(article);

            // Assert
            await handler.Received(expected).Publish(Arg.Any<CardSectionMessage>());
        }
    }
}
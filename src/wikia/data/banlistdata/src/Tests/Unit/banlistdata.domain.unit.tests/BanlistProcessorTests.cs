using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using banlistdata.core.Models;
using banlistdata.domain.Processor;
using banlistdata.domain.Services.Messaging;
using banlistdata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using wikia.Api;
using wikia.Models.Article.AlphabeticalList;
using wikia.Models.Article.Details;
using wikia.Models.Article.Simple;

namespace banlistdata.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistProcessorTests
    {
        private IBanlistDataQueue _banlistDataQueue;
        private IWikiArticle _wikiArticle;
        private BanlistProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _wikiArticle = Substitute.For<IWikiArticle>();
            _banlistDataQueue = Substitute.For<IBanlistDataQueue>();

            _sut = new BanlistProcessor(_wikiArticle, _banlistDataQueue);
        }

        [Test]
        public async Task Given_An_Article_If_Not_Published_To_Queue_IsSuccessful_Should_Be_False()
        {
            // Arrange
            var article = new Article();
            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet{ Items = new Dictionary<string, ExpandedArticle>
            {
                ["key"] = new ExpandedArticle
                {
                    Id = 24324,
                    Abstract = "OCG in effect since September 1, 2007."
                }
            }});

            _wikiArticle.Simple(Arg.Any<long>()).Returns(new ContentResult {Sections = new Section[0]});
            _banlistDataQueue.Publish(Arg.Any<YugiohBanlist>()).Returns(new YugiohBanlistCompletion());

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Article_If_Published_To_Queue_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var article = new Article();
            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet{ Items = new Dictionary<string, ExpandedArticle>
            {
                ["key"] = new ExpandedArticle
                {
                    Id = 24324,
                    Abstract = "OCG in effect since September 1, 2007."
                }
            }});

            _wikiArticle.Simple(Arg.Any<long>()).Returns(new ContentResult {Sections = new Section[0]});
            _banlistDataQueue.Publish(Arg.Any<YugiohBanlist>()).Returns(new YugiohBanlistCompletion{ IsSuccessful = true});

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }


        [Test]
        public async Task Given_An_Article_If_ContentResult_Only_Contains_Sections_With_Title_References_Items_Should_Be_Empty()
        {
            // Arrange
            var article = new Article();
            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet{ Items = new Dictionary<string, ExpandedArticle>
            {
                ["key"] = new ExpandedArticle
                {
                    Id = 24324,
                    Abstract = "OCG in effect since September 1, 2007."
                }
            }});

            _wikiArticle.Simple(Arg.Any<long>()).Returns(new ContentResult
            {
                Sections = new []
                {
                    new Section{ Title = "references"},
                    new Section{ Title = "references"}
                }
            });

            _banlistDataQueue.Publish(Arg.Any<YugiohBanlist>()).Returns(new YugiohBanlistCompletion{ IsSuccessful = true});

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.Banlist.Sections.Should().BeEmpty();
        }

        [Test]
        public async Task Given_An_Article_Banlist_Sections_Should_not_Contain_Invalid_Characters()
        {
            // Arrange
            var expected = "Forbidden";

            var article = new Article();
            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet{ Items = new Dictionary<string, ExpandedArticle>
            {
                ["key"] = new ExpandedArticle
                {
                    Id = 24324,
                    Abstract = "OCG in effect since September 1, 2007."
                }
            }});

            _wikiArticle.Simple(Arg.Any<long>()).Returns(new ContentResult
            {
                Sections = new []
                {
                    new Section { Title = "Forbidden 「フィッシュボーグ－ガンナー」", Content = new []
                    {
                        new SectionContent
                        {
                            Elements = new []
                            {
                                new ListElement
                                {
                                    Text = "Ancient Fairy Dragon"
                                }
                            }
                        }
                    }}
                }
            });

            _banlistDataQueue.Publish(Arg.Any<YugiohBanlist>()).Returns(new YugiohBanlistCompletion{ IsSuccessful = true});

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.Banlist.Sections.First().Title.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_An_Invalid_Article_IsSuccessful_Should_Throw_InvalidOperationException()
        {
            // Arrange
            var article = new Article();
            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet{ Items = new Dictionary<string, ExpandedArticle>()});

            // Act
            Func<Task<ArticleProcessed>> act = () => _sut.Process(article);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
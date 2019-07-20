using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using archetypedata.core.Models;
using archetypedata.domain.Processor;
using archetypedata.domain.Services.Messaging;
using archetypedata.domain.WebPages;
using archetypedata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using wikia.Api;
using wikia.Models.Article;
using wikia.Models.Article.Details;

namespace archetypedata.domain.unit.tests.ArchetypeProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessTests
    {
        private IArchetypeWebPage _archetypeWebPage;
        private IQueue<Archetype> _queue;
        private ArchetypeProcessor _sut;
        private IWikiArticle _wikiArticle;

        [SetUp]
        public void SetUp()
        {
            _archetypeWebPage = Substitute.For<IArchetypeWebPage>();
            _queue = Substitute.For<IQueue<Archetype>>();

            _wikiArticle = Substitute.For<IWikiArticle>();
            _sut = new ArchetypeProcessor(_archetypeWebPage, _queue, _wikiArticle);
        }

        [Test]
        public async Task Given_A_Valid_Article_If_The_Title_Equals_Archetype_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 909890,
                Title = "Archetype",
                Url = "http://yugioh.wikia.com/wiki/Blue-Eyes"
            };

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_Article_If_The_Title_Should_Not_Invoke_ArchetypeThumbnail_Method()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 909890,
                Title = "Archetype",
                Url = "http://yugioh.wikia.com/wiki/Blue-Eyes"
            };

            // Act
            await _sut.Process(article);

            // Assert
            await _archetypeWebPage.DidNotReceive().ArchetypeThumbnail(Arg.Any<long>(), Arg.Any<string>());
        }

        [Test]
        public async Task Given_A_Valid_Article_If_The_Title_Should_Not_Invoke_Publish_Method()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 909890,
                Title = "Archetype",
                Url = "http://yugioh.wikia.com/wiki/Blue-Eyes"
            };

            // Act
            await _sut.Process(article);

            // Assert
            await _queue.DidNotReceive().Publish(Arg.Any<Archetype>());
        }

        [Test]
        public async Task Given_A_Valid_Article_If_Processed_Successfully_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 909890,
                Title = "Blue-Eyes",
                Url = "http://yugioh.wikia.com/wiki/Blue-Eyes"
            };

            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet
            {
                Items = new Dictionary<string, ExpandedArticle>
                {
                    ["690148"] = new ExpandedArticle
                    {
                        Revision = new Revision {Timestamp = 1563318260}
                    }
                }
            });

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Invoke_ArchetypeThumbnail_Once()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 909890,
                Title = "Blue-Eyes",
                Url = "http://yugioh.wikia.com/wiki/Blue-Eyes"
            };

            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet
            {
                Items = new Dictionary<string, ExpandedArticle>
                {
                    ["690148"] = new ExpandedArticle
                    {
                        Revision = new Revision { Timestamp = 1563318260 }
                    }
                }
            });


            // Act
            await _sut.Process(article);

            // Assert
            await _archetypeWebPage.Received(1).ArchetypeThumbnail(Arg.Any<long>(), Arg.Any<string>());
        }

        [Test]
        public async Task Given_A_Valid_Article_Should_Invoke_Publish_Once()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 909890,
                Title = "Blue-Eyes",
                Url = "http://yugioh.wikia.com/wiki/Blue-Eyes"
            };

            _wikiArticle.Details(Arg.Any<int>()).Returns(new ExpandedArticleResultSet
            {
                Items = new Dictionary<string, ExpandedArticle>
                {
                    ["690148"] = new ExpandedArticle
                    {
                        Revision = new Revision { Timestamp = 1563318260 }
                    }
                }
            });


            // Act
            await _sut.Process(article);

            // Assert
            await _queue.Received(1).Publish(Arg.Any<Archetype>());
        }

    }
}

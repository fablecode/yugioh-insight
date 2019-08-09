using System.Collections.Generic;
using cardsectiondata.core.Constants;
using cardsectiondata.domain.ArticleList.Item;
using cardsectiondata.domain.Services.Messaging;
using cardsectiondata.domain.WebPages;
using cardsectiondata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using wikia.Api;
using wikia.Models.Article.AlphabeticalList;

namespace cardsectiondata.domain.unit.tests.ArticleListTests.ItemTests.CardTipItemProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
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

        [TestCase(ArticleCategory.CardTips)]
        public void Given_A_Valid_Category_Should_Return_True(string category)
        {
            // Arrange
            // Act
            var result = _sut.Handles(category);

            // Assert
            result.Should().BeTrue();
        }

        [TestCase("category")]
        public void Given_A_Valid_Category_Should_Return_False(string category)
        {
            // Arrange
            // Act
            var result = _sut.Handles(category);

            // Assert
            result.Should().BeFalse();
        }
    }
}
﻿using article.core.Models;
using article.domain.Services.Messaging;
using article.domain.Settings;
using article.infrastructure.Services.Messaging.Cards;
using article.tests.core;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.unit.tests.ServicesTests.MessagingTests.CardsTests.CardArticleQueueTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class PublishTests
    {
        private CardArticleQueue _sut;
        private IQueue<Article> _queue;

        [SetUp]
        public void SetUp()
        {
            _queue = Substitute.For<IQueue<Article>>();
            _sut = new CardArticleQueue(Substitute.For<IOptions<AppSettings>>(), _queue);
        }

        [Test]
        public async Task Given_An_UnexpandedArticle_Should_Invoke_Publish_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var unexpandedArticle = new UnexpandedArticle();

            // Act
            await _sut.Publish(unexpandedArticle);

            // Assert
            await _queue.Received(expected).Publish(Arg.Is(unexpandedArticle));
        }
    }
}
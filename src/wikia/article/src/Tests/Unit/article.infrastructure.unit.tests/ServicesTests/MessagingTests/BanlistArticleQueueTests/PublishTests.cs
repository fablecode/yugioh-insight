using System.Threading.Tasks;
using article.core.Models;
using article.domain.Services.Messaging;
using article.infrastructure.Services.Messaging;
using article.tests.core;
using NSubstitute;
using NUnit.Framework;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.unit.tests.ServicesTests.MessagingTests.BanlistArticleQueueTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class PublishTests
    {
        private BanlistArticleQueue _sut;
        private IQueue<Article> _queue;

        [SetUp]
        public void SetUp()
        {
            _queue = Substitute.For<IQueue<Article>>();
            _sut = new BanlistArticleQueue(_queue);
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
            await _queue.Received(expected).Publish(Arg.Any<Article>());
        }
    }
}
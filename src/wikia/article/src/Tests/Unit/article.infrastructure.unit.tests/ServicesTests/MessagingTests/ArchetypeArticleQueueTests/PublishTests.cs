using article.core.Models;
using article.domain.Services.Messaging;
using article.infrastructure.Services.Messaging;
using article.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.infrastructure.unit.tests.ServicesTests.MessagingTests.ArchetypeArticleQueueTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class PublishTests
    {
        private ArchetypeArticleQueue _sut;
        private IQueue<Article> _queue;

        [SetUp]
        public void SetUp()
        {
            _queue = Substitute.For<IQueue<Article>>();
            _sut = new ArchetypeArticleQueue(_queue);
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
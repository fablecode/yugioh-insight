using System.Threading.Tasks;
using cardprocessor.core.Models;
using cardprocessor.domain.Queues.Cards;
using cardprocessor.domain.Services.Messaging.Cards;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.ServiceTests.MessagingTests.CardsTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardImageQueueServiceTests
    {
        private ICardImageQueue _cardImageQueue;
        private CardImageQueueService _sut;

        [SetUp]
        public void SetUp()
        {
            _cardImageQueue = Substitute.For<ICardImageQueue>();
            _sut = new CardImageQueueService(_cardImageQueue);
        }

        [Test]
        public async Task Given_A_DownloadImage_Should_Execute_Publish_Method()
        {
            // Arrange
            const int expected = 1;
            var downloadImage = new DownloadImage();

            // Act
            await _sut.Publish(downloadImage);

            // Assert
            await _cardImageQueue.Received(expected).Publish(Arg.Is(downloadImage));
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Cards.Application.MessageConsumers.CardData;
using FluentAssertions;
using NUnit.Framework;

namespace Cards.Application.Unit.Tests.MessageConsumersTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardInformationConsumerHandlerTests
    {
        private CardInformationConsumerHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CardInformationConsumerHandler();
        }

        [Test]
        public async Task Given_Card_Json_If_Json_Is_Processed_Successfully_Is_The_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var cardInformationConsumer = new CardInformationConsumer
            (
                "{\"Name\":\"Amazoness Archer\",\"Types\":\"Warrior / Effect\",\"CardType\":\"Monster\",\"Attribute\":\"Earth\",\"Level\":4,\"Rank\":null,\"PendulumScale\":null,\"AtkDef\":\"1400 / 1000\",\"AtkLink\":null,\"CardNumber\":\"91869203\",\"Materials\":null,\"CardEffectTypes\":\"Ignition\",\"Property\":null,\"Description\":\"You can Tribute 2 monsters; inflict 1200 damage to your opponent.\",\"ImageUrl\":\"https://vignette.wikia.nocookie.net/yugioh/images/0/02/AmazonessArcher-LEDU-EN-C-1E.png\",\"LinkArrows\":null,\"MonsterSubCategoriesAndTypes\":[\"Warrior\",\"Effect\"],\"MonsterLinkArrows\":null}"
            );

            // Act
            var result = await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
using System.Threading.Tasks;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;
using cardprocessor.domain.Repository;
using cardprocessor.domain.Strategies;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.StrategiesTests.TrapCardTypeStrategyTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UpdateTests
    {
        private ICardRepository _cardRepository;
        private TrapCardTypeStrategy _sut;

        [SetUp]
        public void SetUp()
        {
            _cardRepository = Substitute.For<ICardRepository>();

            _sut = new TrapCardTypeStrategy(_cardRepository);
        }

        [Test]
        public async Task Given_A_CardModel_Should_Execute_CardById_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardModel = new CardModel();

            // Act
            await _sut.Update(cardModel);

            // Assert
            await _cardRepository.Received(expected).CardById(Arg.Any<long>());
        }

        [Test]
        public async Task Given_A_CardModel_If_Card_Not_Found_Should_Not_Execute_Update_Method()
        {
            // Arrange
            var cardModel = new CardModel();

            // Act
            await _sut.Update(cardModel);

            // Assert
            await _cardRepository.DidNotReceive().Update(Arg.Any<Card>());
        }

        [Test]
        public async Task Given_A_CardModel_Should_Execute_Update_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardModel = new CardModel();
            _cardRepository.CardById(Arg.Any<long>()).Returns(new Card());

            // Act
            await _sut.Update(cardModel);

            // Assert
            await _cardRepository.Received(expected).Update(Arg.Any<Card>());
        }
    }
}
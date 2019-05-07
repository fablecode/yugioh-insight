using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Strategies;
using cardprocessor.domain.Repository;
using cardprocessor.domain.Services;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.ServiceTests.CardServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardByIdTests
    {
        private IEnumerable<ICardTypeStrategy> _cardTypeStrategies;
        private ICardRepository _cardRepository;
        private CardService _sut;

        [SetUp]
        public void SetUp()
        {
            _cardTypeStrategies = Substitute.For<IEnumerable<ICardTypeStrategy>>();
            _cardRepository = Substitute.For<ICardRepository>();
            _sut = new CardService(_cardTypeStrategies, _cardRepository);
        }

        [Test]
        public async Task Given_A_Card_Id_Should_Invoke_CardById_Method_Once()
        {
            // Arrange
            const long cardId = 4322;

            _cardRepository.CardById(Arg.Any<long>()).Returns(new Card());

            // Act
            await _sut.CardById(cardId);

            // Assert
            await _cardRepository.Received(1).CardById(Arg.Is(cardId));
        }
    }
}
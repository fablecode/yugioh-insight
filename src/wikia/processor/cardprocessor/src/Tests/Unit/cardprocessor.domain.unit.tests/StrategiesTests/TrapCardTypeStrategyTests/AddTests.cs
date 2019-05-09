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
    public class AddTests
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
        public async Task Given_A_CardModel_Should_Execute_Add_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardModel = new CardModel();

            // Act
            await _sut.Add(cardModel);

            // Assert
            await _cardRepository.Received(expected).Add(Arg.Any<Card>());
        }
    }
}
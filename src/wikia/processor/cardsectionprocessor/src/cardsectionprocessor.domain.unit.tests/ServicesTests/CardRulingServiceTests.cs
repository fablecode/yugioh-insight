using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.domain.Repository;
using cardsectionprocessor.domain.Service;
using cardsectionprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardsectionprocessor.domain.unit.tests.ServicesTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardRulingServiceTests
    {
        private ICardRulingRepository _cardRulingRepository;
        private CardRulingService _sut;

        [SetUp]
        public void SetUp()
        {
            _cardRulingRepository = Substitute.For<ICardRulingRepository>();

            _sut = new CardRulingService(_cardRulingRepository);
        }

        [Test]
        public async Task Given_A_CardId_Should_Invoke_DeleteByCardId_Once()
        {
            // Arrange
            const int cardId = 2342;

            // Act
            await _sut.DeleteByCardId(cardId);

            // Assert
            await _cardRulingRepository.Received(1).DeleteByCardId(Arg.Any<long>());
        }

        [Test]
        public async Task Given_A_Collection_Of_Rulings_Should_Invoke_Update_Once()
        {
            // Arrange
            var rulingSections = new List<RulingSection>();

            // Act
            await _sut.Update(rulingSections);

            // Assert
            await _cardRulingRepository.Received(1).Update(Arg.Any<List<RulingSection>>());
        }
    }
}
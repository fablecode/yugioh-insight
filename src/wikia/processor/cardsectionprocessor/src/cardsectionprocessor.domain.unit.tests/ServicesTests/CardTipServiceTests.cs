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
    public class CardTipServiceTests
    {
        private ICardTipRepository _cardTipRepository;
        private CardTipService _sut;

        [SetUp]
        public void SetUp()
        {
            _cardTipRepository = Substitute.For<ICardTipRepository>();

            _sut = new CardTipService(_cardTipRepository);
        }

        [Test]
        public async Task Given_A_CardId_Should_Invoke_DeleteByCardId_Once()
        {
            // Arrange
            const int cardId = 2342;

            // Act
            await _sut.DeleteByCardId(cardId);

            // Assert
            await _cardTipRepository.Received(1).DeleteByCardId(Arg.Any<long>());
        }

        [Test]
        public async Task Given_A_Collection_Of_Tips_Should_Invoke_Update_Once()
        {
            // Arrange
            var tipSections = new List<TipSection>();

            // Act
            await _sut.Update(tipSections);

            // Assert
            await _cardTipRepository.Received(1).Update(Arg.Any<List<TipSection>>());
        }
    }
}
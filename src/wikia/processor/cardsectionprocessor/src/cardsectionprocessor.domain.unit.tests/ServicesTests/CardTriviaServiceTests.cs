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
    public class CardTriviaServiceTests
    {
        private ICardTriviaRepository _cardTriviaRepository;
        private CardTriviaService _sut;

        [SetUp]
        public void SetUp()
        {
            _cardTriviaRepository = Substitute.For<ICardTriviaRepository>();

            _sut = new CardTriviaService(_cardTriviaRepository);
        }

        [Test]
        public async Task Given_A_CardId_Should_Invoke_DeleteByCardId_Once()
        {
            // Arrange
            const int cardId = 2342;

            // Act
            await _sut.DeleteByCardId(cardId);

            // Assert
            await _cardTriviaRepository.Received(1).DeleteByCardId(Arg.Any<long>());
        }

        [Test]
        public async Task Given_A_Collection_Of_Rulings_Should_Invoke_Update_Once()
        {
            // Arrange
            var triviaSections = new List<TriviaSection>();

            // Act
            await _sut.Update(triviaSections);

            // Assert
            await _cardTriviaRepository.Received(1).Update(Arg.Any<List<TriviaSection>>());
        }
    }
}
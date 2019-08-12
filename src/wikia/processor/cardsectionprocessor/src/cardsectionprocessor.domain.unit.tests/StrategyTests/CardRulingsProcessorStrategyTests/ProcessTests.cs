using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models;
using cardsectionprocessor.core.Models.Db;
using cardsectionprocessor.core.Service;
using cardsectionprocessor.domain.Strategy;
using cardsectionprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace cardsectionprocessor.domain.unit.tests.StrategyTests.CardRulingsProcessorStrategyTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessTests
    {
        private ICardRulingService _cardRulingService;
        private ICardService _cardService;
        private CardRulingsProcessorStrategy _sut;

        [SetUp]
        public void SetUp()
        {
            _cardRulingService = Substitute.For<ICardRulingService>();
            _cardService = Substitute.For<ICardService>();

            _sut = new CardRulingsProcessorStrategy(_cardService, _cardRulingService);
        }

        [Test]
        public async Task Given_A_CardSectionMessage_If_Card_Is_Not_Found_IsSuccessful_Should_be_False()
        {
            // Arrange
            var cardSectionMessage = new CardSectionMessage();

            // Act
            var result = await _sut.Process(cardSectionMessage);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_A_CardSectionMessage_If_Card_Is_Not_Found_Should_Return_Errors()
        {
            // Arrange
            var cardSectionMessage = new CardSectionMessage();

            // Act
            var result = await _sut.Process(cardSectionMessage);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_A_CardSectionMessage_Should_Invoke_CardByName_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardSectionMessage = new CardSectionMessage();

            // Act
            await _sut.Process(cardSectionMessage);

            // Assert
            await _cardService.Received(expected).CardByName(Arg.Any<string>());
        }

        [Test]
        public async Task Given_A_CardSectionMessage_Should_Invoke_DeleteByCardId_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardSectionMessage = new CardSectionMessage();
            _cardService.CardByName(Arg.Any<string>()).Returns(new Card());

            // Act
            await _sut.Process(cardSectionMessage);

            // Assert
            await _cardRulingService.Received(expected).DeleteByCardId(Arg.Any<long>());
        }

        [Test]
        public async Task Given_A_CardSectionMessage_Should_Invoke_Update_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardSectionMessage = new CardSectionMessage
            {
                CardSections = new List<CardSection>
                {
                    new CardSection
                    {
                        Name = "Call Of The Haunted",
                        ContentList = new List<string>
                        {
                            "It's a trap!"
                        }
                    }
                }
            };
            _cardService.CardByName(Arg.Any<string>()).Returns(new Card());

            // Act
            await _sut.Process(cardSectionMessage);

            // Assert
            await _cardRulingService.Received(expected).Update(Arg.Any<List<RulingSection>>());
        }
    }
}

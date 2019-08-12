using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectionprocessor.core.Models;
using cardsectionprocessor.core.Strategy;
using cardsectionprocessor.domain.Processor;
using cardsectionprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace cardsectionprocessor.domain.unit.tests.ProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardSectionProcessorTests
    {
        private IEnumerable<ICardSectionProcessorStrategy> _cardSectionProcessorStrategies;
        private CardSectionProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _cardSectionProcessorStrategies = Substitute.For<IEnumerable<ICardSectionProcessorStrategy>>();

            _sut = new CardSectionProcessor(_cardSectionProcessorStrategies);
        }

        [Test]
        public async Task Given_A_Category_And_CardSectionMessage_IsSuccessful_Should_Be_True()
        {
            // Arrange
            const string category = "category";
            var cardSectionMessage = new CardSectionMessage();

            var handler = Substitute.For<ICardSectionProcessorStrategy>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Process(Arg.Any<CardSectionMessage>()).Returns(new CardSectionDataTaskResult<CardSectionMessage> ());

            _cardSectionProcessorStrategies.GetEnumerator().Returns(new List<ICardSectionProcessorStrategy> { handler }.GetEnumerator());

            // Act
            var result = await _sut.Process(category, cardSectionMessage);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Category_And_CardSectionMessage_Should_Invoke_Process_Method_Once()
        {
            // Arrange
            const string category = "category";
            var cardSectionMessage = new CardSectionMessage();

            var handler = Substitute.For<ICardSectionProcessorStrategy>();
            handler.Handles(Arg.Any<string>()).Returns(true);
            handler.Process(Arg.Any<CardSectionMessage>()).Returns(new CardSectionDataTaskResult<CardSectionMessage> ());

            _cardSectionProcessorStrategies.GetEnumerator().Returns(new List<ICardSectionProcessorStrategy> { handler }.GetEnumerator());

            // Act
            await _sut.Process(category, cardSectionMessage);

            // Assert
            await handler.Received(1).Process(Arg.Any<CardSectionMessage>());
        }
    }
}
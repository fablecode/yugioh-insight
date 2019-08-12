using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cardsectionprocessor.application.MessageConsumers.CardInformation;
using cardsectionprocessor.application.MessageConsumers.CardTriviaInformation;
using cardsectionprocessor.tests.core;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NUnit.Framework;

namespace cardsectionprocessor.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardTriviaInformationConsumerHandlerTests
    {
        private IMediator _mediator;
        private CardTriviaInformationConsumerHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new CardTriviaInformationConsumerHandler(_mediator);
        }

        [Test]
        public async Task Given_An_Invalid_CardTriviaInformationConsumer_IsSuccessful_Should_Be_False()
        {
            // Arrange
            var cardTriviaInformationConsumer = new CardTriviaInformationConsumer();
            _mediator.Send(Arg.Any<CardInformationConsumer>(), Arg.Any<CancellationToken>()).Returns(new CardInformationConsumerResult { Errors = new List<string>
            {
                "Something went horriblely wrong."
            }});

            // Act
            var result = await _sut.Handle(cardTriviaInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_CardTriviaInformationConsumer_Errors_List_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var cardTriviaInformationConsumer = new CardTriviaInformationConsumer();
            _mediator.Send(Arg.Any<CardInformationConsumer>(), Arg.Any<CancellationToken>()).Returns(new CardInformationConsumerResult
            {
                Errors = new List<string>
                {
                    "Something went horriblely wrong."
                }
            });

            // Act
            var result = await _sut.Handle(cardTriviaInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_A_Valid_CardTriviaInformationConsumer_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var cardTriviaInformationConsumer = new CardTriviaInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Trivia:Tenyi\"}"
            };

            _mediator.Send(Arg.Any<CardInformationConsumer>(), Arg.Any<CancellationToken>()).Returns( new CardInformationConsumerResult());

            // Act
            var result = await _sut.Handle(cardTriviaInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_CardTriviaInformationConsumer_Should_Execute_Send_Method_Once()
        {
            // Arrange
            var cardTriviaInformationConsumer = new CardTriviaInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Trivia:Tenyi\"}"
            };

            _mediator.Send(Arg.Any<CardInformationConsumer>(), Arg.Any<CancellationToken>()).Returns(new CardInformationConsumerResult());

            // Act
            await _sut.Handle(cardTriviaInformationConsumer, CancellationToken.None);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<CardInformationConsumer>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Given_A_Valid_CardTriviaInformationConsumer_If_Not_Processed_Successfully_Errors_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var cardInformationConsumer = new CardTriviaInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Trivia:Tenyi\"}"
            };

            _mediator.Send(Arg.Any<CardInformationConsumer>(), Arg.Any<CancellationToken>()).Returns(new CardInformationConsumerResult {Errors = new List<string> {"Something went horribly wrong"}});

            // Act
            var result = await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }
    }
}

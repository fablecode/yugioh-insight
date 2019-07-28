using cardsectiondata.application.MessageConsumers.CardTipInformation;
using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using cardsectiondata.tests.core;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cardsectiondata.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardTipInformationConsumerHandlerTests
    {
        private IValidator<CardTipInformationConsumer> _validator;
        private IArticleProcessor _archetypeCardProcessor;
        private CardTipInformationConsumerHandler _sut;
        private ILogger<CardTipInformationConsumerHandler> _logger;

        [SetUp]
        public void SetUp()
        {
            _validator = Substitute.For<IValidator<CardTipInformationConsumer>>();

            _archetypeCardProcessor = Substitute.For<IArticleProcessor>();

            _logger = Substitute.For<ILogger<CardTipInformationConsumerHandler>>();

            _sut = new CardTipInformationConsumerHandler(_archetypeCardProcessor, _validator, _logger);
        }

        [Test]
        public async Task Given_An_Invalid_CardTipInformationConsumer_IsSuccessful_Should_Be_False()
        {
            // Arrange
            var cardTipInformationConsumer = new CardTipInformationConsumer();
            _validator.Validate(Arg.Any<CardTipInformationConsumer>())
                .Returns(new CardTipInformationConsumerValidator().Validate(cardTipInformationConsumer));

            // Act
            var result = await _sut.Handle(cardTipInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_CardTipInformationConsumer_Errors_List_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var cardTipInformationConsumer = new CardTipInformationConsumer();
            _validator.Validate(Arg.Any<CardTipInformationConsumer>())
                .Returns(new CardTipInformationConsumerValidator().Validate(cardTipInformationConsumer));

            // Act
            var result = await _sut.Handle(cardTipInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_A_Valid_CardTipInformationConsumer_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var cardTipInformationConsumer = new CardTipInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Tips:Tenyi\"}"
            };

            _validator.Validate(Arg.Any<CardTipInformationConsumer>())
                .Returns(new CardTipInformationConsumerValidator().Validate(cardTipInformationConsumer));
            _archetypeCardProcessor.Process(Arg.Any<string>(), Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            var result = await _sut.Handle(cardTipInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_CardTipInformationConsumer_Should_Execute_Process_Method_Once()
        {
            // Arrange
            var cardTipInformationConsumer = new CardTipInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Tips:Tenyi\"}"
            };

            _validator.Validate(Arg.Any<CardTipInformationConsumer>())
                .Returns(new CardTipInformationConsumerValidator().Validate(cardTipInformationConsumer));
            _archetypeCardProcessor.Process(Arg.Any<string>(), Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            await _sut.Handle(cardTipInformationConsumer, CancellationToken.None);

            // Assert
            await _archetypeCardProcessor.Received(1).Process(Arg.Any<string>(), Arg.Any<Article>());
        }

        [Test]
        public async Task Given_A_Valid_CardTipInformationConsumer_If_Not_Processed_Successfully_Errors_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var cardTipInformationConsumer = new CardTipInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Tips:Tenyi\"}"
            };

            _validator.Validate(Arg.Any<CardTipInformationConsumer>())
                .Returns(new CardTipInformationConsumerValidator().Validate(cardTipInformationConsumer));
            _archetypeCardProcessor.Process(Arg.Any<string>(), Arg.Any<Article>()).Returns(new ArticleTaskResult { Errors = new List<string> { "Something went horribly wrong" } });

            // Act
            var result = await _sut.Handle(cardTipInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }
    }
}

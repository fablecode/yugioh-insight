using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cardsectiondata.application.MessageConsumers.CardInformation;
using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;
using cardsectiondata.tests.core;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace cardsectiondata.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardInformationConsumerHandlerTests
    {
        private IValidator<CardInformationConsumer> _validator;
        private IArticleProcessor _articleProcessor;
        private CardInformationConsumerHandler _sut;
        private ILogger<CardInformationConsumerHandler> _logger;

        [SetUp]
        public void SetUp()
        {
            _validator = Substitute.For<IValidator<CardInformationConsumer>>();

            _articleProcessor = Substitute.For<IArticleProcessor>();

            _logger = Substitute.For<ILogger<CardInformationConsumerHandler>>();

            _sut = new CardInformationConsumerHandler(_articleProcessor, _validator, _logger);
        }

        [Test]
        public async Task Given_An_Invalid_CardInformationConsumer_IsSuccessful_Should_Be_False()
        {
            // Arrange
            var cardInformationConsumer = new CardInformationConsumer();
            _validator.Validate(Arg.Any<CardInformationConsumer>())
                .Returns(new CardInformationConsumerValidator().Validate(cardInformationConsumer));

            // Act
            var result = await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_CardInformationConsumer_Errors_List_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var cardTipInformationConsumer = new CardInformationConsumer();
            _validator.Validate(Arg.Any<CardInformationConsumer>())
                .Returns(new CardInformationConsumerValidator().Validate(cardTipInformationConsumer));

            // Act
            var result = await _sut.Handle(cardTipInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_A_Valid_CardInformationConsumer_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var cardInformationConsumer = new CardInformationConsumer
            {
                Category = "category",
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Tips:Tenyi\"}"
            };

            _validator.Validate(Arg.Any<CardInformationConsumer>())
                .Returns(new CardInformationConsumerValidator().Validate(cardInformationConsumer));
            _articleProcessor.Process(Arg.Any<string>(), Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            var result = await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_CardInformationConsumer_Should_Execute_Process_Method_Once()
        {
            // Arrange
            var cardInformationConsumer = new CardInformationConsumer
            {
                Category = "category",
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Tips:Tenyi\"}"
            };

            _validator.Validate(Arg.Any<CardInformationConsumer>())
                .Returns(new CardInformationConsumerValidator().Validate(cardInformationConsumer));
            _articleProcessor.Process(Arg.Any<string>(), Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            await _articleProcessor.Received(1).Process(Arg.Any<string>(), Arg.Any<Article>());
        }

        [Test]
        public async Task Given_A_Valid_CardInformationConsumer_If_Not_Processed_Successfully_Errors_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var cardInformationConsumer = new CardInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Card_Tips:Tenyi\"}"
            };

            _validator.Validate(Arg.Any<CardInformationConsumer>())
                .Returns(new CardInformationConsumerValidator().Validate(cardInformationConsumer));
            _articleProcessor.Process(Arg.Any<string>(), Arg.Any<Article>()).Returns(new ArticleTaskResult { Errors = new List<string> { "Something went horribly wrong" } });

            // Act
            var result = await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }
    }
}
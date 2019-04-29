using carddata.application.MessageConsumers.CardInformation;
using carddata.core.Models;
using carddata.core.Processor;
using carddata.tests.core;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace carddata.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardInformationConsumerHandlerTests
    {
        private IArticleProcessor _articleProcessor;
        private IValidator<CardInformationConsumer> _validator;
        private CardInformationConsumerHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _articleProcessor = Substitute.For<IArticleProcessor>();
            _validator = Substitute.For<IValidator<CardInformationConsumer>>();
            _sut = new CardInformationConsumerHandler(_articleProcessor,_validator);
        }

        [Test]
        public async Task Given_An_Invalid_CardInformationConsumer_ArticleConsumerResult_Should_Be_Null()
        {
            // Arrange
            var cardInformationConsumer = new CardInformationConsumer();

            _validator
                .Validate(Arg.Is(cardInformationConsumer))
                .Returns(new CardInformationConsumerValidator().Validate(cardInformationConsumer));

            // Act
            var result = await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            result.ArticleConsumerResult.Should().BeNull();
        }


        [Test]
        public async Task Given_An_Valid_CardInformationConsumer_IsSuccessful_Should_BeTrue()
        {
            // Arrange
            var cardInformationConsumer = new CardInformationConsumer
            {
                Message = $"{{\"Id\":10917,\"Title\":\"Amazoness Archer\",\"Url\":\"https://yugioh.fandom.com/wiki/Amazoness_Archer\"}}"
            };

            _validator
                .Validate(Arg.Is(cardInformationConsumer))
                .Returns(new CardInformationConsumerValidator().Validate(cardInformationConsumer));

            _articleProcessor.Process(Arg.Any<Article>())
                .Returns(new ArticleConsumerResult {IsSuccessfullyProcessed = true});

            // Act
            var result = await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            result.ArticleConsumerResult.IsSuccessfullyProcessed.Should().BeTrue();
        }


        [Test]
        public async Task Given_An_Valid_CardInformationConsumer_Should_Invoke_ArticleProcessor_Process_Once()
        {
            // Arrange
            var cardInformationConsumer = new CardInformationConsumer
            {
                Message = $"{{\"Id\":10917,\"Title\":\"Amazoness Archer\",\"Url\":\"https://yugioh.fandom.com/wiki/Amazoness_Archer\"}}"
            };

            _validator
                .Validate(Arg.Is(cardInformationConsumer))
                .Returns(new CardInformationConsumerValidator().Validate(cardInformationConsumer));

            _articleProcessor.Process(Arg.Any<Article>())
                .Returns(new ArticleConsumerResult {IsSuccessfullyProcessed = true});

            // Act
            await _sut.Handle(cardInformationConsumer, CancellationToken.None);

            // Assert
            await _articleProcessor.Received(1).Process(Arg.Any<Article>());
        }
    }
}

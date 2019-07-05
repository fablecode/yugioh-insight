using System;
using System.Threading;
using System.Threading.Tasks;
using banlistdata.application.MessageConsumers.BanlistInformation;
using banlistdata.core.Exceptions;
using banlistdata.core.Models;
using banlistdata.core.Processor;
using banlistdata.tests.core;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;

namespace banlistdata.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistInformationConsumerHandlerTests
    {
        private IArticleProcessor _articleProcessor;
        private IValidator<BanlistInformationConsumer> _validator;
        private BanlistInformationConsumerHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _articleProcessor = Substitute.For<IArticleProcessor>();
            _validator = Substitute.For<IValidator<BanlistInformationConsumer>>();
            _sut = new BanlistInformationConsumerHandler(_articleProcessor, _validator);
        }

        [Test]
        public async Task Given_An_Invalid_BanlistInformationConsumer_BanlistInformationConsumerResult_Should_Be_Null()
        {
            // Arrange
            var banlistInformationConsumer = new BanlistInformationConsumer();

            _validator
                .Validate(Arg.Is(banlistInformationConsumer))
                .Returns(new BanlistInformationConsumerValidator().Validate(banlistInformationConsumer));

            // Act
            var result = await _sut.Handle(banlistInformationConsumer, CancellationToken.None);

            // Assert
            result.Message.Should().BeNull();
        }

        [Test]
        public async Task Given_An_Valid_BanlistInformationConsumer_IsSuccessful_Should_BeTrue()
        {
            // Arrange
            var banlistInformationConsumer = new BanlistInformationConsumer
            {
                Message = $"{{\"Id\":16022,\"CorrelationId\":\"0606a974-ea5f-436b-a263-ee638762e019\",\"Title\":\"Historic Forbidden/Limited Chart\",\"Url\":null}}"
            };

            _validator
                .Validate(Arg.Is(banlistInformationConsumer))
                .Returns(new BanlistInformationConsumerValidator().Validate(banlistInformationConsumer));

            _articleProcessor.Process(Arg.Any<Article>())
                .Returns(new ArticleConsumerResult { IsSuccessfullyProcessed = true });

            // Act
            var result = await _sut.Handle(banlistInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_An_Valid_BanlistInformationConsumer_Should_Invoke_ArticleProcessor_Process_Once()
        {
            // Arrange
            var banlistInformationConsumer = new BanlistInformationConsumer
            {
                Message = $"{{\"Id\":16022,\"CorrelationId\":\"0606a974-ea5f-436b-a263-ee638762e019\",\"Title\":\"Historic Forbidden/Limited Chart\",\"Url\":null}}"
            };

            _validator
                .Validate(Arg.Is(banlistInformationConsumer))
                .Returns(new BanlistInformationConsumerValidator().Validate(banlistInformationConsumer));

            _articleProcessor.Process(Arg.Any<Article>())
                .Returns(new ArticleConsumerResult { IsSuccessfullyProcessed = true });

            // Act
            await _sut.Handle(banlistInformationConsumer, CancellationToken.None);

            // Assert
            await _articleProcessor.Received(1).Process(Arg.Any<Article>());
        }

        [Test]
        public async Task Given_An_Valid_BanlistInformationConsumer_If_Processing_Failed_IsSuccessful_Should_Be_False()
        {
            // Arrange
            var banlistInformationConsumer = new BanlistInformationConsumer
            {
                Message = $"{{\"Id\":16022,\"CorrelationId\":\"0606a974-ea5f-436b-a263-ee638762e019\",\"Title\":\"Historic Forbidden/Limited Chart\",\"Url\":null}}"
            };

            _validator
                .Validate(Arg.Is(banlistInformationConsumer))
                .Returns(new BanlistInformationConsumerValidator().Validate(banlistInformationConsumer));

            _articleProcessor.Process(Arg.Any<Article>())
                .Returns(new ArticleConsumerResult
                {
                    IsSuccessfullyProcessed = false,
                    Failed = new ArticleException
                    {
                        Exception = new Exception("Error message")
                    }
                });

            // Act
            var result = await _sut.Handle(banlistInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

    }
}

using archetypedata.application.MessageConsumers.ArchetypeCardInformation;
using archetypedata.core.Models;
using archetypedata.core.Processor;
using archetypedata.tests.core;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace archetypedata.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeCardInformationConsumerHandlerTests
    {
        private IValidator<ArchetypeCardInformationConsumer> _validator;
        private IArchetypeCardProcessor _archetypeCardProcessor;
        private ArchetypeCardInformationConsumerHandler _sut;
        private ILogger<ArchetypeCardInformationConsumerHandler> _logger;

        [SetUp]
        public void SetUp()
        {
            _validator = Substitute.For<IValidator<ArchetypeCardInformationConsumer>>();

            _archetypeCardProcessor = Substitute.For<IArchetypeCardProcessor>();

            _logger = Substitute.For<ILogger<ArchetypeCardInformationConsumerHandler>>();

            _sut = new ArchetypeCardInformationConsumerHandler(_archetypeCardProcessor, _validator, _logger);
        }

        [Test]
        public async Task Given_An_Invalid_ArchetypeCardInformationConsumer_IsSuccessful_Should_Be_False()
        {
            // Arrange
            var archetypeCardInformationConsumer = new ArchetypeCardInformationConsumer();
            _validator.Validate(Arg.Any<ArchetypeCardInformationConsumer>())
                .Returns(new ArchetypeCardInformationConsumerValidator().Validate(archetypeCardInformationConsumer));

            // Act
            var result = await _sut.Handle(archetypeCardInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_ArchetypeCardInformationConsumer_Errors_List_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var archetypeCardInformationConsumer = new ArchetypeCardInformationConsumer();
            _validator.Validate(Arg.Any<ArchetypeCardInformationConsumer>())
                .Returns(new ArchetypeCardInformationConsumerValidator().Validate(archetypeCardInformationConsumer));

            // Act
            var result = await _sut.Handle(archetypeCardInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_A_Valid_ArchetypeCardInformationConsumer_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var archetypeCardInformationConsumer = new ArchetypeCardInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Tenyi\"}"
            };

            _validator.Validate(Arg.Any<ArchetypeCardInformationConsumer>())
                .Returns(new ArchetypeCardInformationConsumerValidator().Validate(archetypeCardInformationConsumer));
            _archetypeCardProcessor.Process(Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            var result = await _sut.Handle(archetypeCardInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_ArchetypeCardInformationConsumer_Should_Execute_Process_Method_Once()
        {
            // Arrange
            var archetypeCardInformationConsumer = new ArchetypeCardInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Tenyi\"}"
            };

            _validator.Validate(Arg.Any<ArchetypeCardInformationConsumer>())
                .Returns(new ArchetypeCardInformationConsumerValidator().Validate(archetypeCardInformationConsumer));
            _archetypeCardProcessor.Process(Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            await _sut.Handle(archetypeCardInformationConsumer, CancellationToken.None);

            // Assert
            await _archetypeCardProcessor.Received(1).Process(Arg.Any<Article>());
        }

        [Test]
        public async Task Given_A_Valid_ArchetypeCardInformationConsumer_If_Not_Processed_Successfully_Errors_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var archetypeCardInformationConsumer = new ArchetypeCardInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Tenyi\"}"
            };

            _validator.Validate(Arg.Any<ArchetypeCardInformationConsumer>())
                .Returns(new ArchetypeCardInformationConsumerValidator().Validate(archetypeCardInformationConsumer));
            _archetypeCardProcessor.Process(Arg.Any<Article>()).Returns(new ArticleTaskResult{ Errors = new List<string>{ "Something went horribly wrong"}});

            // Act
            var result = await _sut.Handle(archetypeCardInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }
    }
}

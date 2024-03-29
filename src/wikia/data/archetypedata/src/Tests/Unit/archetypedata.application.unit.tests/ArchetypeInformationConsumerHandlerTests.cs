﻿using System;
using archetypedata.application.MessageConsumers.ArchetypeInformation;
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
using NSubstitute.ExceptionExtensions;

namespace archetypedata.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeInformationConsumerHandlerTests
    {
        private IValidator<ArchetypeInformationConsumer> _validator;
        private IArchetypeProcessor _archetypeProcessor;
        private ArchetypeInformationConsumerHandler _sut;
        private ILogger<ArchetypeInformationConsumerHandler> _logger;

        [SetUp]
        public void SetUp()
        {
            _validator = Substitute.For<IValidator<ArchetypeInformationConsumer>>();

            _archetypeProcessor = Substitute.For<IArchetypeProcessor>();

            _logger = Substitute.For<ILogger<ArchetypeInformationConsumerHandler>>();

            _sut = new ArchetypeInformationConsumerHandler(_archetypeProcessor, _validator, _logger);
        }

        [Test]
        public async Task Given_An_Invalid_ArchetypeInformationConsumer_IsSuccessful_Should_Be_False()
        {
            // Arrange
            var archetypeInformationConsumer = new ArchetypeInformationConsumer();
            _validator.Validate(Arg.Any<ArchetypeInformationConsumer>())
                .Returns(new ArchetypeInformationConsumerValidator().Validate(archetypeInformationConsumer));

            // Act
            var result = await _sut.Handle(archetypeInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_ArchetypeInformationConsumer_Errors_List_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var archetypeInformationConsumer = new ArchetypeInformationConsumer();
            _validator.Validate(Arg.Any<ArchetypeInformationConsumer>())
                .Returns(new ArchetypeInformationConsumerValidator().Validate(archetypeInformationConsumer));

            // Act
            var result = await _sut.Handle(archetypeInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_A_Valid_ArchetypeInformationConsumer_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var archetypeInformationConsumer = new ArchetypeInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Tenyi\"}"
            };

            _validator.Validate(Arg.Any<ArchetypeInformationConsumer>())
                .Returns(new ArchetypeInformationConsumerValidator().Validate(archetypeInformationConsumer));
            _archetypeProcessor.Process(Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            var result = await _sut.Handle(archetypeInformationConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_ArchetypeInformationConsumer_Should_Execute_Process_Method_Once()
        {
            // Arrange
            var archetypeInformationConsumer = new ArchetypeInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Tenyi\"}"
            };

            _validator.Validate(Arg.Any<ArchetypeInformationConsumer>())
                .Returns(new ArchetypeInformationConsumerValidator().Validate(archetypeInformationConsumer));
            _archetypeProcessor.Process(Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            await _sut.Handle(archetypeInformationConsumer, CancellationToken.None);

            // Assert
            await _archetypeProcessor.Received(1).Process(Arg.Any<Article>());
        }

        [Test]
        public async Task Given_A_Valid_ArchetypeInformationConsumer_If_Not_Processed_Successfully_Errors_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var archetypeInformationConsumer = new ArchetypeInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Tenyi\"}"
            };

            _validator.Validate(Arg.Any<ArchetypeInformationConsumer>())
                .Returns(new ArchetypeInformationConsumerValidator().Validate(archetypeInformationConsumer));
            _archetypeProcessor.Process(Arg.Any<Article>()).Returns(new ArticleTaskResult{ Errors = new List<string>{ "Something went horribly wrong"}});

            // Act
            var result = await _sut.Handle(archetypeInformationConsumer, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_A_Valid_ArchetypeInformationConsumer_If_Process_Method_Throws_Exception_Should_Invoke_LogError()
        {
            // Arrange
            const int expected = 1;
            var archetypeInformationConsumer = new ArchetypeInformationConsumer
            {
                Message = "{\"Id\":703544,\"CorrelationId\":\"3e2bf3ca-d903-440c-8cd5-be61c95ae1fc\",\"Title\":\"Tenyi\",\"Url\":\"https://yugioh.fandom.com/wiki/Tenyi\"}"
            };

            _validator.Validate(Arg.Any<ArchetypeInformationConsumer>())
                .Returns(new ArchetypeInformationConsumerValidator().Validate(archetypeInformationConsumer));

            _archetypeProcessor.Process(Arg.Any<Article>()).Throws<ArgumentNullException>();

            // Act
            await _sut.Handle(archetypeInformationConsumer, CancellationToken.None);

            // Assert
            _logger.Received(expected).Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Any<object>(), Arg.Any<Exception>(), Arg.Any<Func<object, Exception, string>>());
        }
    }
}

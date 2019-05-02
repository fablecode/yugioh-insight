using cardprocessor.application.Commands.DownloadImage;
using cardprocessor.core.Models;
using cardprocessor.core.Services.Messaging.Cards;
using cardprocessor.tests.core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cardprocessor.application.unit.tests.Commands
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DownloadImageCommandHandlerTests
    {
        private ICardImageQueueService _cardImageQueueService;
        private IValidator<DownloadImageCommand> _validator;
        private DownloadImageCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _cardImageQueueService = Substitute.For<ICardImageQueueService>();
            _validator = Substitute.For<IValidator<DownloadImageCommand>>();

            _sut = new DownloadImageCommandHandler(_cardImageQueueService, _validator);
        }

        [Test]
        public async Task Given_An_Invalid_DownloadImageCommand_Should_Return_Errors()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand();
            _validator.Validate(Arg.Any<DownloadImageCommand>()).Returns(new ValidationResult
            {
                Errors = { new ValidationFailure("Validation property", "Validation failed")}
            });

            // Act
            var result = await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_An_Valid_DownloadImageCommand_Should_Execute_Publish_Method_Once()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand
            {
                ImageFolderPath = @"c:\windows",
                ImageFileName = "Call Of The Haunted",
                RemoteImageUrl = new Uri("http://filesomewhere/callofthehaunted.png")
            };

            _validator.Validate(Arg.Any<DownloadImageCommand>()).Returns(new DownloadImageCommandValidator().Validate(downloadImageCommand));
            _cardImageQueueService.Publish(Arg.Any<DownloadImage>()).Returns(new CardImageCompletion());

            // Act
            await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            await _cardImageQueueService.Received(1).Publish(Arg.Any<DownloadImage>());
        }

        [Test]
        public async Task Given_An_Valid_DownloadImageCommand_If_Publish_Fails_IsSuccessful_Should_Be_False()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand
            {
                ImageFolderPath = @"c:\windows",
                ImageFileName = "Call Of The Haunted",
                RemoteImageUrl = new Uri("http://filesomewhere/callofthehaunted.png")
            };

            _validator.Validate(Arg.Any<DownloadImageCommand>()).Returns(new DownloadImageCommandValidator().Validate(downloadImageCommand));
            _cardImageQueueService.Publish(Arg.Any<DownloadImage>()).Returns(new CardImageCompletion());

            // Act
            var result = await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Valid_DownloadImageCommand_If_Publish_Succeeds_IsSuccessful_Should_Be_True()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand
            {
                ImageFolderPath = @"c:\windows",
                ImageFileName = "Call Of The Haunted",
                RemoteImageUrl = new Uri("http://filesomewhere/callofthehaunted.png")
            };

            _validator.Validate(Arg.Any<DownloadImageCommand>()).Returns(new DownloadImageCommandValidator().Validate(downloadImageCommand));
            _cardImageQueueService.Publish(Arg.Any<DownloadImage>()).Returns(new CardImageCompletion{ IsSuccessful = true });

            // Act
            var result = await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
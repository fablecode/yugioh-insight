using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using imageprocessor.application.Commands;
using imageprocessor.application.Commands.DownloadImage;
using imageprocessor.application.MessageConsumers.YugiohImage;
using imageprocessor.tests.core;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace imageprocessor.application.unit.tests.MessageConsumersTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ImageConsumerHandlerTests
    {
        private IMediator _mediator;
        private ImageConsumerHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            var logger = Substitute.For<ILogger<ImageConsumerHandler>>();

            _sut = new ImageConsumerHandler(_mediator, logger);
        }

        [Test]
        public async Task Given_A_CardImageConsumer_If_Exception_Is_Thrown_Exception_Variable_Should_Not_Be_Null()
        {
            // Arrange
            var cardImageConsumer = new ImageConsumer
            {
                Message = "{\"RemoteImageUrl\":\"https://vignette.wikia.nocookie.net/yugioh/images/f/f6/AdreusKeeperofArmageddon-BP01-EN-R-1E.png\",\"ImageFileName\":\"Adreus, Keeper of Armageddon\",\"ImageFolderPath\":\"D:\\\\Apps\\\\ygo-api\\\\Images\\\\Cards\"}\r"
            };

            _mediator.Send(Arg.Any<DownloadImageCommand>()).Throws(new Exception());

            // Act
            var result = await _sut.Handle(cardImageConsumer, CancellationToken.None);

            // Assert
            result.Exception.Should().NotBeNull();
        }

        [Test]
        public async Task Given_A_CardImageConsumer_If_Exception_Is_Thrown_IsSuccessful_Should_False()
        {
            // Arrange
            var cardImageConsumer = new ImageConsumer
            {
                Message = "{\"RemoteImageUrl\":\"https://vignette.wikia.nocookie.net/yugioh/images/f/f6/AdreusKeeperofArmageddon-BP01-EN-R-1E.png\",\"ImageFileName\":\"Adreus, Keeper of Armageddon\",\"ImageFolderPath\":\"D:\\\\Apps\\\\ygo-api\\\\Images\\\\Cards\"}\r"
            };

            _mediator.Send(Arg.Any<DownloadImageCommand>()).Throws(new Exception());

            // Act
            var result = await _sut.Handle(cardImageConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_A_CardImageConsumer_Should_Process_CardImage_Successfully()
        {
            // Arrange
            var cardImageConsumer = new ImageConsumer
            {
                Message = "{\"RemoteImageUrl\":\"https://vignette.wikia.nocookie.net/yugioh/images/f/f6/AdreusKeeperofArmageddon-BP01-EN-R-1E.png\",\"ImageFileName\":\"Adreus, Keeper of Armageddon\",\"ImageFolderPath\":\"D:\\\\Apps\\\\ygo-api\\\\Images\\\\Cards\"}\r"
            };

            _mediator.Send(Arg.Any<DownloadImageCommand>()).Returns(new CommandResult {IsSuccessful = true});

            // Act

            var result = await _sut.Handle(cardImageConsumer, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
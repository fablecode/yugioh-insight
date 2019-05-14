using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using imageprocessor.application.Commands.DownloadImage;
using imageprocessor.core.Models;
using imageprocessor.core.Services;
using imageprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace imageprocessor.application.unit.tests.CommandsTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DownloadImageCommandHandlerTests
    {
        private IValidator<DownloadImageCommand> _validator;
        private IFileSystemService _fileSystemService;
        private DownloadImageCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _validator = Substitute.For<IValidator<DownloadImageCommand>>();
            _fileSystemService = Substitute.For<IFileSystemService>();

            _sut = new DownloadImageCommandHandler(_fileSystemService, _validator);
        }

        [Test]
        public async Task Given_An_Invalid_DownloadImageCommand_Validation_Should_Fail()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand();
            _validator.Validate(Arg.Is(downloadImageCommand)).Returns(new ValidationResult { Errors = { new ValidationFailure("property", "Failed")}});

            // Act
            var result = await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_DownloadImageCommand_Error_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand();
            _validator.Validate(Arg.Is(downloadImageCommand)).Returns(new ValidationResult { Errors = { new ValidationFailure("property", "Failed")}});

            // Act
            var result = await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_An_Valid_DownloadImageCommand_Should_Process_Command_Successfully()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand
            {
                ImageFileName = "Adreus, Keeper of Armageddon",
                ImageFolderPath = @"D:\Apps\ygo-api\Images\Cards",
                RemoteImageUrl = new Uri("https://vignette.wikia.nocookie.net/yugioh/images/f/f6/AdreusKeeperofArmageddon-BP01-EN-R-1E.png")

            };

            _validator.Validate(Arg.Is(downloadImageCommand)).Returns(new ValidationResult());
            _fileSystemService.Download(Arg.Any<Uri>(), Arg.Any<string>()).Returns(new DownloadedFile{ ContentType = "image/png" });

            // Act
            var result = await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_An_Valid_DownloadImageCommand_Should_Invoke_Download_Method_Once()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand
            {
                ImageFileName = "Adreus, Keeper of Armageddon",
                ImageFolderPath = @"D:\Apps\ygo-api\Images\Cards",
                RemoteImageUrl = new Uri("https://vignette.wikia.nocookie.net/yugioh/images/f/f6/AdreusKeeperofArmageddon-BP01-EN-R-1E.png")

            };

            _validator.Validate(Arg.Is(downloadImageCommand)).Returns(new ValidationResult());
            _fileSystemService.Download(Arg.Any<Uri>(), Arg.Any<string>()).Returns(new DownloadedFile{ ContentType = "image/png" });

            // Act
            await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            await _fileSystemService.Received(1).Download(Arg.Any<Uri>(), Arg.Any<string>());
        }

        [Test]
        public async Task Given_An_Valid_DownloadImageCommand_Should_Invoke_Exists_Method_Once()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand
            {
                ImageFileName = "Adreus, Keeper of Armageddon",
                ImageFolderPath = @"D:\Apps\ygo-api\Images\Cards",
                RemoteImageUrl = new Uri("https://vignette.wikia.nocookie.net/yugioh/images/f/f6/AdreusKeeperofArmageddon-BP01-EN-R-1E.png")

            };

            _validator.Validate(Arg.Is(downloadImageCommand)).Returns(new ValidationResult());
            _fileSystemService.Download(Arg.Any<Uri>(), Arg.Any<string>()).Returns(new DownloadedFile{ ContentType = "image/png" });

            // Act
            await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            _fileSystemService.Received(1).Exists(Arg.Any<string>());
        }

        [Test]
        public async Task Given_An_Valid_DownloadImageCommand_If_Image_Does_Not_Exist_Should_Not_Invoke_Delete_Method()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand
            {
                ImageFileName = "Adreus, Keeper of Armageddon",
                ImageFolderPath = @"D:\Apps\ygo-api\Images\Cards",
                RemoteImageUrl = new Uri("https://vignette.wikia.nocookie.net/yugioh/images/f/f6/AdreusKeeperofArmageddon-BP01-EN-R-1E.png")

            };

            _validator.Validate(Arg.Is(downloadImageCommand)).Returns(new ValidationResult());
            _fileSystemService.Download(Arg.Any<Uri>(), Arg.Any<string>()).Returns(new DownloadedFile{ ContentType = "image/png" });

            // Act
            await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            _fileSystemService.DidNotReceive().Delete(Arg.Any<string>());
        }

        [Test]
        public async Task Given_An_Valid_DownloadImageCommand_If_Image_Exists_Should_Invoke_Delete_Method_Once()
        {
            // Arrange
            var downloadImageCommand = new DownloadImageCommand
            {
                ImageFileName = "Adreus, Keeper of Armageddon",
                ImageFolderPath = @"D:\Apps\ygo-api\Images\Cards",
                RemoteImageUrl = new Uri("https://vignette.wikia.nocookie.net/yugioh/images/f/f6/AdreusKeeperofArmageddon-BP01-EN-R-1E.png")

            };

            _validator.Validate(Arg.Is(downloadImageCommand)).Returns(new ValidationResult());
            _fileSystemService.Download(Arg.Any<Uri>(), Arg.Any<string>()).Returns(new DownloadedFile{ ContentType = "image/png" });
            _fileSystemService.Exists(Arg.Any<string>()).Returns(true);

            // Act
            await _sut.Handle(downloadImageCommand, CancellationToken.None);

            // Assert
            _fileSystemService.Received(1).Delete(Arg.Any<string>());
        }
    }
}
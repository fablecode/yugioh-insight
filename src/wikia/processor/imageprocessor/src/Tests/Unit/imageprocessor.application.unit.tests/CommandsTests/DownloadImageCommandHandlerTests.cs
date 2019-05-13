using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using imageprocessor.application.Commands.DownloadImage;
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
    }
}
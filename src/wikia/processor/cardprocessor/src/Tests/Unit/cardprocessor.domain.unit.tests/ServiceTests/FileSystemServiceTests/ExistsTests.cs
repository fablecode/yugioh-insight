using cardprocessor.domain.Services;
using cardprocessor.domain.SystemIO;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.ServiceTests.FileSystemServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ExistsTests
    {
        private IFileSystem _fileSystem;
        private FileSystemService _sut;

        [SetUp]
        public void SetUp()
        {
            _fileSystem = Substitute.For<IFileSystem>();
            _sut = new FileSystemService(_fileSystem);
        }

        [Test]
        public void Given_A_LocalFileFullPath_Should_Invoke_Rename_Exists_Once()
        {
            // Arrange
            const string localFileFullPath = @"c:\images\picture.png";

            // Act
            _sut.Exists(localFileFullPath);

            // Assert
            _fileSystem.Exists(Arg.Is(localFileFullPath));
        }

    }
}
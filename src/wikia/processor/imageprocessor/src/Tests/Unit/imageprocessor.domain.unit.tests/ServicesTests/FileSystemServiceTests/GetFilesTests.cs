using imageprocessor.domain.Services;
using imageprocessor.domain.SystemIO;
using imageprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace imageprocessor.domain.unit.tests.ServicesTests.FileSystemServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class GetFilesTests
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
        public void Given_A_Path_And_SearchPattern_Should_Invoke_GetFiles_Method_Once()
        {
            // Arrange
            const string path = @"c:\images\pic.png";
            const string searchPattern = "*pic";

            // Act
            _sut.GetFiles(path, searchPattern);

            // Assert
            _fileSystem.Received(1).GetFiles(Arg.Is(path), searchPattern);
        }

    }
}
﻿using imageprocessor.domain.Services;
using imageprocessor.domain.SystemIO;
using imageprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace imageprocessor.domain.unit.tests.ServicesTests.FileSystemServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class RenameTests
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
        public void Given_A_LocalFileFullPath_Should_Invoke_Rename_Method_Once()
        {
            // Arrange
            const string oldNameFullPath = @"c:\images\pic.png";
            const string localFileFullPath = @"c:\images\picture.png";

            // Act
            _sut.Rename(oldNameFullPath, localFileFullPath);

            // Assert
            _fileSystem.Received(1).Rename(Arg.Is(oldNameFullPath), Arg.Is(localFileFullPath));
        }

    }
}
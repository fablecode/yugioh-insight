﻿using cardprocessor.tests.core;
using imageprocessor.domain.Services;
using imageprocessor.domain.SystemIO;
using NSubstitute;
using NUnit.Framework;

namespace imageprocessor.domain.unit.tests.ServicesTests.FileSystemServiceTests
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
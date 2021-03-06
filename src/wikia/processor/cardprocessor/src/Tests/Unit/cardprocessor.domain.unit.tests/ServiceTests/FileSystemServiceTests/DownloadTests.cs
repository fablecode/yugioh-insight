﻿using System;
using System.Threading.Tasks;
using cardprocessor.domain.Services;
using cardprocessor.domain.SystemIO;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.ServiceTests.FileSystemServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DownloadTests
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
        public async Task Given_A_RemoteFileUrl_And_A_LocalFileFullPath_Should_Invoke_Download_Method_Once()
        {
            // Arrange
            var remoteFileUrl = new Uri("http://www.google.com");
            const string localFileFullPath = @"c:\images\pic.png";

            // Act
            await _sut.Download(remoteFileUrl, localFileFullPath);

            // Assert
            await _fileSystem.Received(1).Download(Arg.Is(remoteFileUrl), Arg.Is(localFileFullPath));
        }
    }
}
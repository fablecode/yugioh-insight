﻿using System;
using System.Threading.Tasks;
using banlistdata.core.Models;
using banlistdata.core.Processor;
using banlistdata.domain.Processor;
using banlistdata.tests.core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace banlistdata.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArticleProcessorTests
    {
        private IArticleDataFlow _articleDataFlow;
        private ArticleProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _articleDataFlow = Substitute.For<IArticleDataFlow>();
            _sut = new ArticleProcessor(_articleDataFlow, Substitute.For<ILogger<ArticleProcessed>>());
        }

        [Test]
        public async Task Given_An_Article_If_Processed_And_Errors_Do_Not_Occur_IsSuccessfullyProcessed_Should_Be_True()
        {
            // Arrange
            var article = new Article();
            _articleDataFlow.ProcessDataFlow(article).Returns(new ArticleCompletion { IsSuccessful = true});

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.IsSuccessfullyProcessed.Should().BeTrue();
        }

        [Test]
        public async Task Given_An_Article_If_Processed_And_Errors_Occur_IsSuccessfullyProcessed_Should_Be_False()
        {
            // Arrange
            var article = new Article();
            _articleDataFlow.ProcessDataFlow(article).Returns(new ArticleCompletion( ));

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.IsSuccessfullyProcessed.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Article_If_Processed_And_An_Exception_Occur_IsSuccessfullyProcessed_Should_Be_False()
        {
            // Arrange
            var article = new Article();
            _articleDataFlow.ProcessDataFlow(article).Throws(new Exception());

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.IsSuccessfullyProcessed.Should().BeFalse();
        }

    }
}

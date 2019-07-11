using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using article.application.ScheduledTasks.Archetype;
using article.core.ArticleList.Processor;
using article.core.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace article.application.unit.tests.ScheduledTasksTests
{
    [TestFixture]
    public class ArchetypeInformationTaskHandlerTests
    {
        private ArchetypeInformationTaskHandler _sut;
        private IArticleCategoryProcessor _articleCategoryProcessor;

        [SetUp]
        public void Setup()
        {
            _articleCategoryProcessor = Substitute.For<IArticleCategoryProcessor>();

            _sut = new ArchetypeInformationTaskHandler(_articleCategoryProcessor, new ArchetypeInformationTaskValidator());
        }

        [Test]
        public async Task Given_An_Invalid_ArchetypeInformationTask_Should_Return_Errors()
        {
            // Arrange
            var task = new ArchetypeInformationTask();

            // Act
            var result = await _sut.Handle(task, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_An_Invalid_ArchetypeInformationTask_Should_Not_Execute_Process()
        {
            // Arrange
            var task = new ArchetypeInformationTask();

            // Act
            await _sut.Handle(task, CancellationToken.None);

            // Assert
            await _articleCategoryProcessor.DidNotReceive().Process(Arg.Any<IEnumerable<string>>(), Arg.Any<int>());
        }

        [Test]
        public async Task Given_An_Valid_ArchetypeInformationTask_Should_Execute_Process()
        {
            // Arrange
            var task = new ArchetypeInformationTask
            {
                Category = "category",
                PageSize = 1
            };

            _articleCategoryProcessor.Process(Arg.Any<string>(), Arg.Any<int>()).Returns(new ArticleBatchTaskResult());

            // Act
            await _sut.Handle(task, CancellationToken.None);

            // Assert
            await _articleCategoryProcessor.Received(1).Process(Arg.Any<string>(), Arg.Any<int>());
        }
    }
}
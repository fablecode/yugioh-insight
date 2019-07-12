using article.application.ScheduledTasks.Articles;
using article.core.ArticleList.Processor;
using article.tests.core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace article.application.unit.tests.ScheduledTasksTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArticleInformationTaskHandlerTests
    {
        private IArticleCategoryProcessor _articleCategoryProcessor;
        private IValidator<ArticleInformationTask> _validator;
        private ArticleInformationTaskHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _articleCategoryProcessor = Substitute.For<IArticleCategoryProcessor>();
            _validator = Substitute.For<IValidator<ArticleInformationTask>>();

            _sut = new ArticleInformationTaskHandler
            (
                _articleCategoryProcessor,
                _validator
            );
        }

        [Test]
        public async Task Given_A_ArticleInformationTask_If_Validation_Fails_Should_Return_Errors()
        {
            // Arrange
            _validator.Validate(Arg.Any<ArticleInformationTask>()).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("propertyName", "failed")
            }));

            // Act
            var result = await _sut.Handle(new ArticleInformationTask(), CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }


        [Test]
        public async Task Given_A_ArticleInformationTask_If_Validation_Fails_Should_Not_Invoke_ArticleCategoryProcessor_Process_Method()
        {
            // Arrange
            _validator.Validate(Arg.Any<ArticleInformationTask>()).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("propertyName", "failed")
            }));

            // Act
            await _sut.Handle(new ArticleInformationTask(), CancellationToken.None);

            // Assert
            await _articleCategoryProcessor.DidNotReceive().Process(Arg.Any<string>(), Arg.Any<int>());
        }


        [Test]
        public async Task Given_A_ArticleInformationTask_If_Validation_Is_Successful_Should_Be_true()
        {
            // Arrange
            var banlistInformationTask = new ArticleInformationTask { Category = "banlist", PageSize = 8};
            _validator.Validate(Arg.Any<ArticleInformationTask>()).Returns(new ArticleInformationTaskValidator().Validate(banlistInformationTask));

            // Act

            var result = await _sut.Handle(banlistInformationTask, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
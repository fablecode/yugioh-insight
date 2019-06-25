using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using article.application.ScheduledTasks.CardInformation;
using article.core.ArticleList.Processor;
using article.core.Models;
using article.tests.core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;

namespace article.application.unit.tests.ScheduledTasksTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardInformationTaskHandlerTests
    {
        private IArticleCategoryProcessor _articleCategoryProcessor;
        private IValidator<CardInformationTask> _validator;
        private CardInformationTaskHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _articleCategoryProcessor = Substitute.For<IArticleCategoryProcessor>();

            _validator = Substitute.For<IValidator<CardInformationTask>>();

            _sut = new CardInformationTaskHandler(_articleCategoryProcessor, _validator);
        }

        [Test]
        public async Task Given_A_CardInformationTask_If_Validation_Fails_Should_Return_Errors()
        {
            // Arrange
            _validator.Validate(Arg.Any<CardInformationTask>()).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("propertyName", "failed")
            }));

            // Act
            var result = await _sut.Handle(new CardInformationTask(), CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_A_CardInformationTask_If_Validation_Fails_Should_Not_Invoke_Process_Method()
        {
            // Arrange
            _validator.Validate(Arg.Any<CardInformationTask>()).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("propertyName", "failed")
            }));

            // Act
            await _sut.Handle(new CardInformationTask(), CancellationToken.None);

            // Assert
            await _articleCategoryProcessor.DidNotReceive().Process(Arg.Any<string>(), Arg.Any<int>());
        }

        [Test]
        public async Task Given_A_Valid_CardInformationTask_Is_Successful_Should_Be_True()
        {
            // Arrange
            var cardInformationTask = new CardInformationTask{ Categories = new List<string>{ "category"}, PageSize = 1};

            _validator.Validate(Arg.Any<CardInformationTask>()).Returns(new CardInformationTaskValidator().Validate(cardInformationTask));
            _articleCategoryProcessor.Process(Arg.Any<IEnumerable<string>>(), Arg.Any<int>()).Returns(new List<ArticleBatchTaskResult>());

            // Act
            var result = await _sut.Handle(cardInformationTask, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_Valid_CardInformationTask_Should_Invoke_Process_Once()
        {
            // Arrange
            var cardInformationTask = new CardInformationTask { Categories = new List<string>{ "category"}, PageSize = 1};

            _validator.Validate(Arg.Any<CardInformationTask>()).Returns(new CardInformationTaskValidator().Validate(cardInformationTask));
            _articleCategoryProcessor.Process(Arg.Any<IEnumerable<string>>(), Arg.Any<int>()).Returns(new List<ArticleBatchTaskResult>());

            // Act
            await _sut.Handle(cardInformationTask, CancellationToken.None);

            // Assert
            await _articleCategoryProcessor.Received(1).Process(Arg.Any<IEnumerable<string>>(), Arg.Any<int>());
        }
    }
}
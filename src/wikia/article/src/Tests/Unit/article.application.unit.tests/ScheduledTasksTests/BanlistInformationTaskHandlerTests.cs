using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using article.application.ScheduledTasks.CardInformation;
using article.application.ScheduledTasks.LatestBanlist;
using article.core.ArticleList.Processor;
using article.core.Enums;
using article.domain.Banlist.Processor;
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
    public class BanlistInformationTaskHandlerTests
    {
        private IArticleCategoryProcessor _articleCategoryProcessor;
        private IValidator<BanlistInformationTask> _validator;
        private IBanlistProcessor _banlistProcessor;
        private BanlistInformationTaskHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _articleCategoryProcessor = Substitute.For<IArticleCategoryProcessor>();
            _validator = Substitute.For<IValidator<BanlistInformationTask>>();
            _banlistProcessor = Substitute.For<IBanlistProcessor>();

            _sut = new BanlistInformationTaskHandler
            (
                _articleCategoryProcessor,
                _validator,
                _banlistProcessor
            );
        }

        [Test]
        public async Task Given_A_BanlistInformationTask_If_Validation_Fails_Should_Return_Errors()
        {
            // Arrange
            _validator.Validate(Arg.Any<BanlistInformationTask>()).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("propertyName", "failed")
            }));

            // Act
            var result = await _sut.Handle(new BanlistInformationTask(), CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeNullOrEmpty();
        }


        [Test]
        public async Task Given_A_BanlistInformationTask_If_Validation_Fails_Should_Not_Invoke_ArticleCategoryProcessor_Process_Method()
        {
            // Arrange
            _validator.Validate(Arg.Any<BanlistInformationTask>()).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("propertyName", "failed")
            }));

            // Act
            await _sut.Handle(new BanlistInformationTask(), CancellationToken.None);

            // Assert
            await _articleCategoryProcessor.DidNotReceive().Process(Arg.Any<string>(), Arg.Any<int>());
        }

        [Test]
        public async Task Given_A_BanlistInformationTask_If_Validation_Fails_Should_Not_Invoke_BanlistProcessor_Process_Method()
        {
            // Arrange
            _validator.Validate(Arg.Any<BanlistInformationTask>()).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("propertyName", "failed")
            }));

            // Act
            await _sut.Handle(new BanlistInformationTask(), CancellationToken.None);

            // Assert
            await _banlistProcessor.DidNotReceive().Process(Arg.Any<BanlistType>());
        }


        [Test]
        public async Task Given_A_BanlistInformationTask_If_Validation_Is_Succesful_Should_Be_true()
        {
            // Arrange
            var banlistInformationTask = new BanlistInformationTask{ Category = "card", PageSize = 8};
            _validator.Validate(Arg.Any<BanlistInformationTask>()).Returns(new BanlistInformationTaskValidator().Validate(banlistInformationTask));

            // Act

            var result = await _sut.Handle(banlistInformationTask, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_BanlistInformationTask_If_Validation_Is_Succesful_Should_Invoke_BanlistProcessor_Process_Method_Twice()
        {
            // Arrange
            const int expected = 2;
            var banlistInformationTask = new BanlistInformationTask { Category = "card", PageSize = 8 };
            _validator.Validate(Arg.Any<BanlistInformationTask>()).Returns(new BanlistInformationTaskValidator().Validate(banlistInformationTask));

            // Act

            await _sut.Handle(banlistInformationTask, CancellationToken.None);

            // Assert
            await _banlistProcessor.Received(expected).Process(Arg.Any<BanlistType>());
        }

    }
}
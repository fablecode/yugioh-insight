using article.application.ScheduledTasks.Articles;
using article.tests.core;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace article.application.unit.tests.ScheduledTasksTests.ValidationTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArticleInformationTaskValidatorTests
    {
        private ArticleInformationTaskValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ArticleInformationTaskValidator();
        }

        [TestCaseSource(nameof(InvalidCategories))]
        public void Given_Invalid_Categories_Validation_Should_Fail(string category)
        {
            // Arrange
            var inputModel = new ArticleInformationTask { Category = category };
            var result = _sut.TestValidate(inputModel);

            // Act
            void Act() => result.ShouldHaveValidationErrorFor(ci => ci.Category);

            // Assert
            Act();
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(501)]
        public void Given_Invalid_PageSize_Validation_Should_Fail(int pageSize)
        {
            // Arrange
            var inputModel = new ArticleInformationTask { PageSize = pageSize };
            var result = _sut.TestValidate(inputModel);

            // Act
            void Act() => result.ShouldHaveValidationErrorFor(ci => ci.PageSize);

            // Assert
            Act();
        }

        #region private helpers

        private static readonly object[] InvalidCategories =
        {
            null,
            "",
            " "
        };

        #endregion
    }
}
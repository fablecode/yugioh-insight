using article.application.ScheduledTasks.Archetype;
using article.tests.core;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace article.application.unit.tests.ScheduledTasksTests.ValidationTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeInformationTaskValidatorTests
    {
        private ArchetypeInformationTaskValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ArchetypeInformationTaskValidator();
        }

        [TestCaseSource(nameof(InvalidCategories))]
        public void Given_Invalid_Categories_Validation_Should_Fail(string category)
        {
            // Arrange
            var inputModel = new ArchetypeInformationTask { Category = category };
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
            var inputModel = new ArchetypeInformationTask { PageSize = pageSize };
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
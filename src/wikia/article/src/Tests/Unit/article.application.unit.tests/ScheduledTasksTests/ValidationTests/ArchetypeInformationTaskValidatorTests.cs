using System;
using article.application.ScheduledTasks.Archetype;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace article.application.unit.tests.ScheduledTasksTests.ValidationTests
{
    [TestFixture]
    public class ArchetypeInformationTaskValidatorTests
    {
        private ArchetypeInformationTaskValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ArchetypeInformationTaskValidator();
        }

        [TestCaseSource(nameof(_invalidCategories))]
        public void Given_Invalid_Categories_Validation_Should_Fail(string category)
        {
            // Arrange
            var inputModel = new ArchetypeInformationTask { Category = category };

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(ci => ci.Category, inputModel);

            // Assert
            act.Invoke();
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Given_Invalid_PageSize_Validation_Should_Fail(int pageSize)
        {
            // Arrange
            var inputModel = new ArchetypeInformationTask { PageSize = pageSize };

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(ci => ci.PageSize, inputModel);

            // Assert
            act.Invoke();
        }

        #region private helpers

        static object[] _invalidCategories =
        {
            null,
            "",
            " "
        };

        #endregion
    }
}
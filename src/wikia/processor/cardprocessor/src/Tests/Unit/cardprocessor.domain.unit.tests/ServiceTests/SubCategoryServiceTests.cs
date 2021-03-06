﻿using System.Threading.Tasks;
using cardprocessor.domain.Repository;
using cardprocessor.domain.Services;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class SubCategoryServiceTests
    {
        private ISubCategoryRepository _subCategoryRepository;
        private SubCategoryService _sut;

        [SetUp]
        public void SetUp()
        {
            _subCategoryRepository = Substitute.For<ISubCategoryRepository>();
            _sut = new SubCategoryService(_subCategoryRepository);
        }

        [Test]
        public async Task Should_Invoke_AllSubCategories_Method_Once()
        {
            // Arrange
            // Act
            await _sut.AllSubCategories();

            // Assert
            await _subCategoryRepository.Received(1).AllSubCategories();
        }
    }
}
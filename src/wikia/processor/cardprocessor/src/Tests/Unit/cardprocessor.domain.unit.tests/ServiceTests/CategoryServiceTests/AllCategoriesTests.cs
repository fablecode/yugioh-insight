using System.Threading.Tasks;
using cardprocessor.domain.Repository;
using cardprocessor.domain.Services;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.ServiceTests.CategoryServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AllCategoriesTests
    {
        private ICategoryRepository _categoryRepository;
        private CategoryService _sut;

        [SetUp]
        public void SetUp()
        {
            _categoryRepository = Substitute.For<ICategoryRepository>();
            _sut = new CategoryService(_categoryRepository);
        }

        [Test]
        public async Task Should_Invoke_AllCategories_Method_Once()
        {
            // Arrange
            // Act
            await _sut.AllCategories();

            // Assert
            await _categoryRepository.Received(1).AllCategories();
        }
    }
}
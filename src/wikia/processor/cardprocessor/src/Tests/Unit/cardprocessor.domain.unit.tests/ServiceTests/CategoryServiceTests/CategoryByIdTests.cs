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
    public class CategoryByIdTests
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
        public async Task Given_A_CategoryId_Should_Invoke_CategoryById_Method_Once()
        {
            // Arrange
            const int categoryId = 8434;

            // Act
            await _sut.CategoryById(categoryId);

            // Assert
            await _categoryRepository.CategoryById(Arg.Is(categoryId));

        }
    }
}
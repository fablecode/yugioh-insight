using System.Threading.Tasks;
using cardprocessor.core.Models.Db;
using cardprocessor.domain.Repository;
using cardprocessor.domain.Services;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.ServiceTests.CategoryServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AddTests
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
        public async Task Given_A_Category_Should_Invoke_Add_Method_Once()
        {
            // Arrange
            var category = new Category();

            // Act
            await _sut.Add(category);
            
            // Assert
            await _categoryRepository.Received(1).Add(Arg.Is(category));
        }
    }
}
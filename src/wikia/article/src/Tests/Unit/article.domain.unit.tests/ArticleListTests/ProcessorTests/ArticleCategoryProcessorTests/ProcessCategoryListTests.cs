using article.core.ArticleList.DataSource;
using article.core.ArticleList.Processor;
using article.domain.ArticleList.Processor;
using article.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace article.domain.unit.tests.ArticleListTests.ProcessorTests.ArticleCategoryProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessCategoryListTests
    {
        private IArticleCategoryDataSource _articleCategoryDataSource;
        private IArticleBatchProcessor _articleBatchProcessor;
        private ArticleCategoryProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _articleCategoryDataSource = Substitute.For<IArticleCategoryDataSource>();
            _articleBatchProcessor = Substitute.For<IArticleBatchProcessor>();
            _sut = new ArticleCategoryProcessor(_articleCategoryDataSource, _articleBatchProcessor);
        }

        [Test]
        public async Task Given_A_CategoryList_And_PageSize_Should_Invoke_Producer_Method_3_Times()
        {
            // Arrange
            const int expected = 3;
            var categories = new List<string> { "category1", "category2", "category3" };
            const int pageSize = 10;

            // Act
            await _sut.Process(categories, pageSize);

            // Assert
            await _articleCategoryDataSource.Received(expected).Producer(Arg.Any<string>(), Arg.Any<int>()).ToListAsync();
        }

    }
}
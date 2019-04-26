﻿using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using article.core.ArticleList.DataSource;
using article.core.ArticleList.Processor;
using article.core.Models;
using article.domain.ArticleList.Processor;
using article.tests.core;
using NSubstitute;
using NUnit.Framework;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.unit.tests.ArticleListTests.ProcessorTests.ArticleCategoryProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessCategoryTests
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
        public async Task Given_A_Category_And_PageSize_Should_Invoke_Producer_Method_Once()
        {
            // Arrange
            const int expected = 1;
            const string category = "category";
            const int pageSize = 10;

            // Act
            await _sut.Process(category, pageSize);

            // Assert
            await _articleCategoryDataSource.Received(expected).Producer(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<BufferBlock<UnexpandedArticle[]>>());
        }
    }
}
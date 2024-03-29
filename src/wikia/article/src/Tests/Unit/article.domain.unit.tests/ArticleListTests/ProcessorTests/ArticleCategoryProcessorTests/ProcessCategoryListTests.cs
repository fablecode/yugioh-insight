﻿using article.core.ArticleList.DataSource;
using article.core.ArticleList.Processor;
using article.domain.ArticleList.Processor;
using article.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using article.core.Models;
using FluentAssertions;
using wikia.Models.Article.AlphabeticalList;

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

        [Test]
        public async Task Given_A_CategoryList_And_PageSize_Should_Invoke_Process_Method_3_Times()
        {
            // Arrange
            const int expected = 3;
            var categories = new List<string> { "category1", "category2", "category3" };
            const int pageSize = 10;

            _articleCategoryDataSource
                .Producer(Arg.Any<string>(), Arg.Any<int>())
                .Returns
                (
                    new List<UnexpandedArticle[]>{ new UnexpandedArticle[0] }.ToAsyncEnumerable(), 
                    new List<UnexpandedArticle[]> { new UnexpandedArticle[0] }.ToAsyncEnumerable(), 
                    new List<UnexpandedArticle[]> { new UnexpandedArticle[0] }.ToAsyncEnumerable()
                );

            _articleBatchProcessor.Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle[]>())
                .Returns(new ArticleBatchTaskResult());

            // Act
            await _sut.Process(categories, pageSize);

            // Assert
            await _articleBatchProcessor.Received(expected).Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle[]>());
        }

        [Test]
        public async Task Given_A_CategoryList_And_PageSize_Number_Of_Processed_Items_Should_Be_3()
        {
            // Arrange
            const int expected = 3;
            var categories = new List<string> { "category1", "category2", "category3" };
            const int pageSize = 10;

            _articleCategoryDataSource
                .Producer(Arg.Any<string>(), Arg.Any<int>())
                .Returns
                (
                    new List<UnexpandedArticle[]>{ new UnexpandedArticle[0] }.ToAsyncEnumerable(), 
                    new List<UnexpandedArticle[]> { new UnexpandedArticle[0] }.ToAsyncEnumerable(), 
                    new List<UnexpandedArticle[]> { new UnexpandedArticle[0] }.ToAsyncEnumerable()
                );

            _articleBatchProcessor.Process(Arg.Any<string>(), Arg.Any<UnexpandedArticle[]>())
                .Returns(new ArticleBatchTaskResult { Processed = 1 });

            // Act
            var result = await _sut.Process(categories, pageSize);

            // Assert
            result.Sum(a => a.Processed).Should().Be(expected);
        }
    }
}
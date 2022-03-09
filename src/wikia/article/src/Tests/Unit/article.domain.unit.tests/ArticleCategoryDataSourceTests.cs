using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using article.core.ArticleList.DataSource;
using article.domain.ArticleList.DataSource;
using article.tests.core;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using wikia.Api;
using wikia.Models.Article;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArticleCategoryDataSourceTests
    {
        private IWikiArticleList _articleList;
        private IArticleCategoryDataSource _sut;

        [SetUp]
        public void Setup()
        {
            _articleList = Substitute.For<IWikiArticleList>();
            _sut = new ArticleCategoryDataSource(_articleList);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_A_Invalid_Category_Should_Throw_ArgumentException(string category)
        {
            // Arrange

            // Act
            Func<Task<List<UnexpandedArticle[]>>> act = () => _sut.Producer(category, 500).ToListAsync();

            // Assert
            act.Should().ThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task Given_Batches_Of_UnexpandedArticles_Should_Process_All_Batches()
        {
            // Arrange
            const int expected = 5;
            const int pageSize = 100;

            var fixture = new Fixture();

            var articleBatch1 = fixture.Build<UnexpandedListArticleResultSet>().With(x => x.Items, new Fixture { RepeatCount = pageSize }.Create<UnexpandedArticle[]>()).Create();
            var articleBatch2 = fixture.Build<UnexpandedListArticleResultSet>().With(x => x.Items, new Fixture { RepeatCount = pageSize }.Create<UnexpandedArticle[]>()).Create();
            var articleBatch3 = fixture.Build<UnexpandedListArticleResultSet>().With(x => x.Items, new Fixture { RepeatCount = pageSize }.Create<UnexpandedArticle[]>()).Create();
            var articleBatch4 = fixture.Build<UnexpandedListArticleResultSet>().With(x => x.Items, new Fixture { RepeatCount = pageSize }.Create<UnexpandedArticle[]>()).Create();
            var articleBatch5 = fixture.Build<UnexpandedListArticleResultSet>().With(x => x.Items, new Fixture { RepeatCount = pageSize }.Create<UnexpandedArticle[]>()).Create();

            // Set Last page
            articleBatch5.Offset = null;

            _articleList
                .AlphabeticalList(Arg.Any<ArticleListRequestParameters>())
                .ReturnsForAnyArgs
                (
                    articleBatch1,
                articleBatch2,
                    articleBatch3,
                    articleBatch4,
                    articleBatch5
                );

            // Act
            var result = await _sut.Producer("category", 500).ToListAsync();


            // Assert
            result.Count.Should().Be(expected);
        }

    }

    public static class AsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var list = new List<T>();
            await foreach (var item in enumerable)
            {
                list.Add(item);
            }
            return list;
        }
        public static async Task<T[]> ToArrayAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var result = await ToListAsync(enumerable);
            return result.ToArray();
        }
        public static async Task<T> SingleOrDefault<T>(this IAsyncEnumerable<T> enumerable)
        {
            var result = await ToListAsync(enumerable);
            return result.SingleOrDefault();
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                yield return await Task.FromResult(item);
            }
        }
    }
}

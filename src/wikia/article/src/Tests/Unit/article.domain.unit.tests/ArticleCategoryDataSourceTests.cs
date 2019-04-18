using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
            Action act = () => _sut.Producer(category, 500, new BufferBlock<UnexpandedArticle[]>()).Wait();

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Given_A_Invalid_ITargetBlock_Should_Throw_ArgumentException()
        {
            // Arrange

            // Act
            Action act = () => _sut.Producer("category", 500, null).Wait();

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public async Task Given_Batches_Of_UnexpandedArticles_Should_Process_All_Batches()
        {
            // Arrange
            const int expected = 5;
            const int pageSize = 100;
            var articleBatchBufferBlock = new BufferBlock<UnexpandedArticle[]>();

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
            await _sut.Producer("category", 500, articleBatchBufferBlock);


            // Assert
            articleBatchBufferBlock.Count.Should().Be(expected);
        }

    }
}

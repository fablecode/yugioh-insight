using articledata.domain.ArticleList.Processor;
using articledata.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using articledata.core.ArticleList.Processor;
using articledata.core.Models;
using wikia.Models.Article.AlphabeticalList;

namespace articledata.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArticleProcessorTests
    {
        private ArticleProcessor _sut;
        private IArticleItemProcessor _processor;

        [SetUp]
        public void SetUp()
        {
            _processor = Substitute.For<IArticleItemProcessor>();
            _processor.Handles(Arg.Any<string>()).Returns(true);
            _processor.ProcessItem(Arg.Any<UnexpandedArticle>()).Returns(new ArticleTaskResult());

            _sut = new ArticleProcessor(new List<IArticleItemProcessor> { _processor });
        }

        [Test]
        public async Task Given_A_Category_And_An_Article_Should_Invoke_ProcessItem_Method_Once()
        {
            // Arrange
            const int expected = 1;
            const string category = "category";

            // Act
            await _sut.Process(category, new UnexpandedArticle());

            // Assert
            await _processor.Received(expected).ProcessItem(Arg.Any<UnexpandedArticle>());
        }
    }
}
using cardsectiondata.core.Models;
using cardsectiondata.domain.ArticleList.Processor;
using cardsectiondata.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using cardsectiondata.core.Processor;

namespace cardsectiondata.domain.unit.tests.ArticleListTests
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

            _sut = new ArticleProcessor(new List<IArticleItemProcessor> {_processor});
        }

        [Test]
        public async Task Given_A_Category_And_An_Article_Should_Invoke_ProcessItem_Method_Once()
        {
            // Arrange
            const int expected = 1;
            const string category = "category";

            _processor.Handles(Arg.Any<string>()).Returns(true);
            _processor.ProcessItem(Arg.Any<Article>()).Returns(new ArticleTaskResult());

            // Act
            await _sut.Process(category, new Article());

            // Assert
            await _processor.Received(expected).ProcessItem(Arg.Any<Article>());
        }
    }
}

using cardsectiondata.core.Constants;
using cardsectiondata.core.Processor;
using cardsectiondata.domain.ArticleList.Item;
using cardsectiondata.domain.Services.Messaging;
using cardsectiondata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace cardsectiondata.domain.unit.tests.ArticleListTests.ItemTests.CardRulingItemProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private IEnumerable<IQueue> _queues;
        private CardRulingItemProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _queues = Substitute.For<IEnumerable<IQueue>>();

            _sut = new CardRulingItemProcessor(Substitute.For<ICardSectionProcessor>(), _queues);
        }

        [TestCase(ArticleCategory.CardRulings)]
        public void Given_A_Valid_Category_Should_Return_True(string category)
        {
            // Arrange
            // Act
            var result = _sut.Handles(category);

            // Assert
            result.Should().BeTrue();
        }

        [TestCase("category")]
        public void Given_A_Valid_Category_Should_Return_False(string category)
        {
            // Arrange
            // Act
            var result = _sut.Handles(category);

            // Assert
            result.Should().BeFalse();
        }
    }
}
using System;
using System.Threading.Tasks;
using archetypedata.core.Models;
using archetypedata.domain.Processor;
using archetypedata.domain.Services.Messaging;
using archetypedata.domain.WebPages;
using archetypedata.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace archetypedata.domain.unit.tests.ArchetypeCardProcessorTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ProcessTests
    {
        private IArchetypeWebPage _archetypeWebPage;
        private IQueue<ArchetypeCard> _queue;
        private ArchetypeCardProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _archetypeWebPage = Substitute.For<IArchetypeWebPage>();
            _queue = Substitute.For<IQueue<ArchetypeCard>>();

            _sut = new ArchetypeCardProcessor(_archetypeWebPage, _queue);
        }

        [Test]
        public async Task Given_A_ArchetypeCard_If_Processed_Successfully_IsSuccessful_Property_Should_Be_True()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 423423,
                Title = "List of \"Clear Wing\" cards",
                Url = "http://yugioh.wikia.com/wiki/List_of_\"Clear_Wing\"_cards"
            };

            // Act
            var result = await _sut.Process(article);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_A_ArchetypeCard_Should_Invoke_Publish_Method_Once()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 423423,
                Title = "List of \"Clear Wing\" cards",
                Url = "http://yugioh.wikia.com/wiki/List_of_\"Clear_Wing\"_cards"
            };

            // Act
            await _sut.Process(article);

            // Assert
            await _queue.Received(1).Publish(Arg.Any<ArchetypeCard>());
        }

        [Test]
        public async Task Given_A_ArchetypeCard_Should_Invoke_Cards_Method_Once()
        {
            // Arrange
            var article = new Article
            {
                CorrelationId = Guid.NewGuid(),
                Id = 423423,
                Title = "List of \"Clear Wing\" cards",
                Url = "http://yugioh.wikia.com/wiki/List_of_\"Clear_Wing\"_cards"
            };

            // Act
            await _sut.Process(article);

            // Assert
            _archetypeWebPage.Received(1).Cards(Arg.Any<Uri>());
        }
    }
}
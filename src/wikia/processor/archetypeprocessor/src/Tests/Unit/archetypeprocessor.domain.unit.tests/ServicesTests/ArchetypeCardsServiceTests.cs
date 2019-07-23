using System.Collections.Generic;
using System.Threading.Tasks;
using archetypeprocessor.domain.Repository;
using archetypeprocessor.domain.Services;
using archetypeprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace archetypeprocessor.domain.unit.tests.ServicesTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeCardsServiceTests
    {
        private IArchetypeCardsRepository _archetypeCardsRepository;
        private ArchetypeCardsService _sut;

        [SetUp]
        public void SetUp()
        {
            _archetypeCardsRepository = Substitute.For<IArchetypeCardsRepository>();

            _sut = new ArchetypeCardsService(_archetypeCardsRepository);
        }

        [Test]
        public async Task Given_An_ArchetypeId_And_Cards_Should_Invoke_Update_Method_Once()
        {
            // Arrange
            const long archetypeId = 234243;
            var cards = new List<string>();

            // Act
            await _sut.Update(archetypeId, cards);

            // Assert
            await _archetypeCardsRepository.Received(1).Update(Arg.Is(archetypeId), Arg.Is(cards));
        }
    }
}
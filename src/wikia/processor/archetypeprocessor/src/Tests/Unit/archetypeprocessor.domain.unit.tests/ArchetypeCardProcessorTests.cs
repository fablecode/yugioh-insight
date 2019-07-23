using System.Collections.Generic;
using System.Threading.Tasks;
using archetypeprocessor.core.Models;
using archetypeprocessor.core.Models.Db;
using archetypeprocessor.core.Services;
using archetypeprocessor.domain.Processor;
using archetypeprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace archetypeprocessor.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeCardProcessorTests
    {
        private IArchetypeService _archetypeService;
        private IArchetypeCardsService _archetypeCardsService;
        private ArchetypeCardProcessor _sut;

        [SetUp]
        public void SetUp()
        {
            _archetypeService = Substitute.For<IArchetypeService>();
            _archetypeCardsService = Substitute.For<IArchetypeCardsService>();

            _sut = new ArchetypeCardProcessor(_archetypeService, _archetypeCardsService);
        }

        [Test]
        public async Task Given_An_ArchetypeCardMessage_If_Archetype_Is_Not_Found_Should_Invoke_Update_Method()
        {
            // Arrange
            var archetypeCardMessage = new ArchetypeCardMessage
            {
                ArchetypeName = "Blue-Eyes White Dragon"
            };

            // Act
            await _sut.Process(archetypeCardMessage);

            // Assert
            await _archetypeCardsService.DidNotReceive().Update(Arg.Any<long>(), Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public async Task Given_An_ArchetypeCardMessage_If_Archetype_Is_Found_Should_Invoke_Update_Method_Once()
        {
            // Arrange
            var archetypeCardMessage = new ArchetypeCardMessage
            {
                ArchetypeName = "Blue-Eyes White Dragon"
            };

            _archetypeService.ArchetypeByName(Arg.Any<string>()).Returns(new Archetype());

            // Act
            await _sut.Process(archetypeCardMessage);

            // Assert
            await _archetypeCardsService.Received(1).Update(Arg.Any<long>(), Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public async Task Given_An_ArchetypeCardMessage_Should_Always_Invoke_ArchetypeByName_Method_Once()
        {
            // Arrange
            var archetypeCardMessage = new ArchetypeCardMessage
            {
                ArchetypeName = "Blue-Eyes White Dragon"
            };

            _archetypeService.ArchetypeByName(Arg.Any<string>()).Returns(new Archetype());

            // Act
            await _sut.Process(archetypeCardMessage);

            // Assert
            await _archetypeService.Received(1).ArchetypeByName(Arg.Any<string>());
        }
    }
}
using System.Threading.Tasks;
using archetypeprocessor.core.Models.Db;
using archetypeprocessor.domain.Repository;
using archetypeprocessor.domain.Services;
using archetypeprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace archetypeprocessor.domain.unit.tests.ServicesTests.ArchetypeServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UpdateTests
    {
        private IArchetypeRepository _archetypeRepository;
        private ArchetypeService _sut;

        [SetUp]
        public void SetUp()
        {
            _archetypeRepository = Substitute.For<IArchetypeRepository>();

            _sut = new ArchetypeService(_archetypeRepository);
        }

        [Test]
        public async Task Given_An_Archetype_Should_Invoke_Update_Method_Once()
        {
            // Arrange
            var archetype = new Archetype();

            // Act
            await _sut.Update(archetype);

            // Assert
            await _archetypeRepository.Received(1).Update(Arg.Is(archetype));
        }
    }
}
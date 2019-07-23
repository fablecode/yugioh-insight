using System.Threading.Tasks;
using archetypeprocessor.domain.Repository;
using archetypeprocessor.domain.Services;
using archetypeprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace archetypeprocessor.domain.unit.tests.ServicesTests.ArchetypeServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ArchetypeByIdTests
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
        public async Task Given_An_Archetype_Id_Should_Invoke_ArchetypeById_Method_Once()
        {
            // Arrange
            const long archetypeId = 23424;

            // Act
            await _sut.ArchetypeById(archetypeId);

            // Assert
            await _archetypeRepository.Received(1).ArchetypeById(Arg.Is(archetypeId));
        }
    }
}
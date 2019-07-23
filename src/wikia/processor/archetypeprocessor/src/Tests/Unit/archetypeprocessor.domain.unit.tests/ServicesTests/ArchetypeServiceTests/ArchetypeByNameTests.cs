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
    public class ArchetypeByNameTests
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
        public async Task Given_An_Archetype_Name_Should_Invoke_ArchetypeByName_Method_Once()
        {
            // Arrange
            const string archetypeName = "Blue-Eyes";

            // Act
            await _sut.ArchetypeByName(archetypeName);

            // Assert
            await _archetypeRepository.Received(1).ArchetypeByName(Arg.Is(archetypeName));
        }
    }
}
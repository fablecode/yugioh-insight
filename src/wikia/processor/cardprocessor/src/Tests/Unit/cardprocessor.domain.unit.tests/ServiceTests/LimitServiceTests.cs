using System.Threading.Tasks;
using cardprocessor.domain.Repository;
using cardprocessor.domain.Services;
using cardprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class LimitServiceTests
    {
        private ILimitRepository _limitRepository;
        private LimitService _sut;

        [SetUp]
        public void SetUp()
        {
            _limitRepository = Substitute.For<ILimitRepository>();
            _sut = new LimitService(_limitRepository);
        }

        [Test]
        public async Task Should_Invoke_AllLimits_Method_Once()
        {
            // Arrange
            // Act
            await _sut.AllLimits();

            // Assert
            await _limitRepository.Received(1).AllLimits();
        }
    }
}
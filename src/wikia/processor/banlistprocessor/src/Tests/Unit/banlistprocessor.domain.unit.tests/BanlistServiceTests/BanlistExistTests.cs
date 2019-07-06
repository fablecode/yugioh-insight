using System.Threading.Tasks;
using AutoMapper;
using banlistprocessor.core.Services;
using banlistprocessor.domain.Mappings.Profiles;
using banlistprocessor.domain.Repository;
using banlistprocessor.domain.Services;
using banlistprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace banlistprocessor.domain.unit.tests.BanlistServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistExistTests
    {
        private BanlistService _sut;
        private IBanlistRepository _banlistRepository;
        private IBanlistCardService _banlistCardService;
        private IFormatRepository _formatRepository;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new BanlistProfile()); }
            );

            var mapper = config.CreateMapper();

            _banlistRepository = Substitute.For<IBanlistRepository>();
            _banlistCardService = Substitute.For<IBanlistCardService>();
            _formatRepository = Substitute.For<IFormatRepository>();

            _sut = new BanlistService
            (
                _banlistCardService, 
                _banlistRepository,
                _formatRepository,
                mapper
            );
        }

        [Test]
        public async Task Given_A_Banlist_Id_Should_Invoke_BanlistExist_Method_Once()
        {
            // Arrange
            const int expected = 1;
            const int id = 123;

            // Act
            await _sut.BanlistExist(id);

            // Assert
            await _banlistRepository.Received(expected).BanlistExist(Arg.Any<int>());
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using banlistprocessor.core.Models;
using banlistprocessor.core.Models.Db;
using banlistprocessor.domain.Repository;
using banlistprocessor.domain.Services;
using banlistprocessor.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace banlistprocessor.domain.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistCardServiceTests
    {
        private ILimitRepository _limitRepository;
        private ICardRepository _cardRepository;
        private IBanlistCardRepository _banlistCardRepository;
        private BanlistCardService _sut;

        [SetUp]
        public void SetUp()
        {
            _limitRepository = Substitute.For<ILimitRepository>();
            _cardRepository = Substitute.For<ICardRepository>();
            _banlistCardRepository = Substitute.For<IBanlistCardRepository>();

            _sut = new BanlistCardService
            (
                _limitRepository,
                _cardRepository,
                _banlistCardRepository
            );
        }

        [Test]
        public async Task Given_A_BanlistId_And_BanlistSections_Should_Invoke_Update_Method_Once()
        {
            // Arrange
            var yugiohBanlistSections = new List<YugiohBanlistSection>
            {
                new YugiohBanlistSection
                {
                    Title = "Forbidden",
                    Content = new List<string>{ "Dark Hole"}
                },
                new YugiohBanlistSection
                {
                    Title = "Limited",
                    Content = new List<string>{ "Raigeki"}
                },
                new YugiohBanlistSection
                {
                    Title = "Semi-Limited",
                    Content = new List<string>{ "One For One"}
                },
                new YugiohBanlistSection
                {
                    Title = "Unlimited",
                    Content = new List<string>{ "Macro"}
                }
            };

            _limitRepository.GetAll().Returns(new List<Limit>
            {
                new Limit
                {
                    Id = 1,
                    Name = "Forbidden"
                },
                new Limit
                {
                    Id = 2,
                    Name = "Limited"
                },
                new Limit
                {
                    Id = 3,
                    Name = "Semi-Limited"
                },
                new Limit
                {
                    Id = 4,
                    Name = "Unlimited"
                }
            });

            _cardRepository.CardByName(Arg.Any<string>()).Returns(new Card
            {
                Id = 432
            });

            // Act
            await _sut.Update(123, yugiohBanlistSections);

            // Assert
            await _banlistCardRepository.Received(1).Update(Arg.Any<long>(), Arg.Any<List<BanlistCard>>());
        }
    }
}
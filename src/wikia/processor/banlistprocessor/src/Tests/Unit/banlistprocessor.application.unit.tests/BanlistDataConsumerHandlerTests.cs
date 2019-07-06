using System;
using System.Threading;
using System.Threading.Tasks;
using banlistprocessor.application.MessageConsumers.BanlistData;
using banlistprocessor.core.Models;
using banlistprocessor.core.Models.Db;
using banlistprocessor.core.Services;
using banlistprocessor.tests.core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace banlistprocessor.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistDataConsumerHandlerTests
    {
        private IBanlistService _banlistService;
        private BanlistDataConsumerHandler _sut;
        private ILogger<BanlistDataConsumerHandler> _logger;

        [SetUp]
        public void SetUp()
        {
            _banlistService = Substitute.For<IBanlistService>();

            _logger = Substitute.For<ILogger<BanlistDataConsumerHandler>>();
            _sut = new BanlistDataConsumerHandler(_banlistService, _logger);
        }

        [Test]
        public async Task Given_An_Invalid_Should_Invoke_LogError_Method_Once()
        {
            // Arrange
            // Act
            await _sut.Handle(new BanlistDataConsumer(), CancellationToken.None);

            // Assert
            _logger.Received().Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), Arg.Any<object>(), Arg.Any<Exception>(), Arg.Any<Func<object, Exception, string>>());

        }

        [Test]
        public async Task Given_A_Valid_Message_Banlist_Property_Should_Not_Be_Null()
        {
            // Arrange
            var banlistDataConsumer = new BanlistDataConsumer
            {
                Message = "{\"ArticleId\":642752,\"Title\":\"April 2000 Lists\",\"BanlistType\":\"Ocg\",\"StartDate\":\"2000-04-01T00:00:00\",\"Sections\":[{\"Title\":\"April 2000 Lists\",\"Content\":[]},{\"Title\":\"Full Lists\",\"Content\":[]},{\"Title\":\"Limited\",\"Content\":[\"Change of Heart\",\"Dark Hole\",\"Exodia the Forbidden One\",\"Last Will\",\"Left Arm of the Forbidden One\",\"Left Leg of the Forbidden One\",\"Mirror Force\",\"Pot of Greed\",\"Raigeki\",\"Right Arm of the Forbidden One\",\"Right Leg of the Forbidden One\"]},{\"Title\":\"Semi-Limited\",\"Content\":[\"Graceful Charity\",\"Harpie's Feather Duster\",\"Monster Reborn\"]}]}"
            };
            _banlistService.Add(Arg.Any<YugiohBanlist>()).Returns(new Banlist());

            // Act
            var result = await _sut.Handle(banlistDataConsumer, CancellationToken.None);

            // Assert
            result.YugiohBanlist.Should().NotBeNull();
        }


        [Test]
        public async Task Given_A_Valid_Message_BanlistExist_Method_Should_Invoke_Once()
        {
            // Arrange
            const int expected = 1;
            var banlistDataConsumer = new BanlistDataConsumer
            {
                Message = "{\"ArticleId\":642752,\"Title\":\"April 2000 Lists\",\"BanlistType\":\"Ocg\",\"StartDate\":\"2000-04-01T00:00:00\",\"Sections\":[{\"Title\":\"April 2000 Lists\",\"Content\":[]},{\"Title\":\"Full Lists\",\"Content\":[]},{\"Title\":\"Limited\",\"Content\":[\"Change of Heart\",\"Dark Hole\",\"Exodia the Forbidden One\",\"Last Will\",\"Left Arm of the Forbidden One\",\"Left Leg of the Forbidden One\",\"Mirror Force\",\"Pot of Greed\",\"Raigeki\",\"Right Arm of the Forbidden One\",\"Right Leg of the Forbidden One\"]},{\"Title\":\"Semi-Limited\",\"Content\":[\"Graceful Charity\",\"Harpie's Feather Duster\",\"Monster Reborn\"]}]}"
            };
            _banlistService.Add(Arg.Any<YugiohBanlist>()).Returns(new Banlist());

            // Act
            await _sut.Handle(banlistDataConsumer, CancellationToken.None);

            // Assert
            await _banlistService.Received(expected).BanlistExist(Arg.Any<int>());
        }


        [Test]
        public async Task Given_A_Valid_Message_If_Banlist_Exists_Should_Invoke_Add_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var banlistDataConsumer = new BanlistDataConsumer
            {
                Message = "{\"ArticleId\":642752,\"Title\":\"April 2000 Lists\",\"BanlistType\":\"Ocg\",\"StartDate\":\"2000-04-01T00:00:00\",\"Sections\":[{\"Title\":\"April 2000 Lists\",\"Content\":[]},{\"Title\":\"Full Lists\",\"Content\":[]},{\"Title\":\"Limited\",\"Content\":[\"Change of Heart\",\"Dark Hole\",\"Exodia the Forbidden One\",\"Last Will\",\"Left Arm of the Forbidden One\",\"Left Leg of the Forbidden One\",\"Mirror Force\",\"Pot of Greed\",\"Raigeki\",\"Right Arm of the Forbidden One\",\"Right Leg of the Forbidden One\"]},{\"Title\":\"Semi-Limited\",\"Content\":[\"Graceful Charity\",\"Harpie's Feather Duster\",\"Monster Reborn\"]}]}"
            };
            _banlistService.BanlistExist(Arg.Any<int>()).Returns(false);
            _banlistService.Add(Arg.Any<YugiohBanlist>()).Returns(new Banlist());

            // Act
            await _sut.Handle(banlistDataConsumer, CancellationToken.None);

            // Assert
            await _banlistService.Received(expected).Add(Arg.Any<YugiohBanlist>());
        }

        [Test]
        public async Task Given_A_Valid_Message_If_Banlist_Does_Not_Exists_Should_Invoke_Update_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var banlistDataConsumer = new BanlistDataConsumer
            {
                Message = "{\"ArticleId\":642752,\"Title\":\"April 2000 Lists\",\"BanlistType\":\"Ocg\",\"StartDate\":\"2000-04-01T00:00:00\",\"Sections\":[{\"Title\":\"April 2000 Lists\",\"Content\":[]},{\"Title\":\"Full Lists\",\"Content\":[]},{\"Title\":\"Limited\",\"Content\":[\"Change of Heart\",\"Dark Hole\",\"Exodia the Forbidden One\",\"Last Will\",\"Left Arm of the Forbidden One\",\"Left Leg of the Forbidden One\",\"Mirror Force\",\"Pot of Greed\",\"Raigeki\",\"Right Arm of the Forbidden One\",\"Right Leg of the Forbidden One\"]},{\"Title\":\"Semi-Limited\",\"Content\":[\"Graceful Charity\",\"Harpie's Feather Duster\",\"Monster Reborn\"]}]}"
            };

            _banlistService.BanlistExist(Arg.Any<int>()).Returns(true);
            _banlistService.Update(Arg.Any<YugiohBanlist>()).Returns(new Banlist());

            // Act
            await _sut.Handle(banlistDataConsumer, CancellationToken.None);

            // Assert
            await _banlistService.Received(expected).Update(Arg.Any<YugiohBanlist>());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using banlistprocessor.core.Models;
using banlistprocessor.core.Models.Db;
using banlistprocessor.core.Services;
using banlistprocessor.domain.Mappings.Profiles;
using banlistprocessor.domain.Repository;
using banlistprocessor.domain.Services;
using banlistprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace banlistprocessor.domain.unit.tests.BanlistServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AddBanlistTests
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
        public async Task Given_A_YugiohBanlist_Should_Invoke_FormatByAcronym_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var yugiohBanlist = new YugiohBanlist();

            _formatRepository.FormatByAcronym(Arg.Any<string>()).Returns(new Format());
            _banlistRepository.Add(Arg.Any<Banlist>()).Returns(new Banlist());

            // Act
            await _sut.Add(yugiohBanlist);

            // Assert
            await _formatRepository.Received(expected).FormatByAcronym(Arg.Any<string>());
        }

        [Test]
        public void Given_A_YugiohBanlist_If_Format_Is_Not_Found_Should_Throw_ArgumentException()
        {
            // Arrange
            var yugiohBanlist = new YugiohBanlist();

            _banlistRepository.Add(Arg.Any<Banlist>()).Returns(new Banlist());

            // Act
            Func<Task<Banlist>> act = () => _sut.Add(yugiohBanlist);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public async Task Given_A_YugiohBanlist_Should_Invoke_BanlistRepository_Add_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var yugiohBanlist = new YugiohBanlist();

            _formatRepository.FormatByAcronym(Arg.Any<string>()).Returns(new Format());
            _banlistRepository.Add(Arg.Any<Banlist>()).Returns(new Banlist());

            // Act
            await _sut.Add(yugiohBanlist);

            // Assert
            await _banlistRepository.Received(expected).Add(Arg.Any<Banlist>());
        }

        [Test]
        public async Task Given_A_YugiohBanlist_Should_Invoke_BanlistCardService_Update_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var yugiohBanlist = new YugiohBanlist();

            _formatRepository.FormatByAcronym(Arg.Any<string>()).Returns(new Format());
            _banlistRepository.Add(Arg.Any<Banlist>()).Returns(new Banlist());

            // Act
            await _sut.Add(yugiohBanlist);

            // Assert
            await _banlistCardService.Received(expected).Update(Arg.Any<long>(),Arg.Any<List<YugiohBanlistSection>>());
        }
    }
}
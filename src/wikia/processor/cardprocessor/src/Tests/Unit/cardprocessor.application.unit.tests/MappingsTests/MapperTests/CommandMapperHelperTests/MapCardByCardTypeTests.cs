﻿using System;
using AutoMapper;
using cardprocessor.application.Dto;
using cardprocessor.application.Enums;
using cardprocessor.application.Mappings.Mappers;
using cardprocessor.application.Mappings.Profiles;
using cardprocessor.core.Models.Db;
using cardprocessor.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.MappingsTests.MapperTests.CommandMapperHelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MapCardByCardTypeTests
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new CardProfile()); }
            );

            _mapper = config.CreateMapper();
        }

        [Test]
        public void Given_An_Invalid_CardType_Should_Throw_ArgumentOutOfRange_Exception()
        {
            // Arrange
            const YgoCardType cardType = (YgoCardType)7;

            var card = new Card { Id = 23424 };

            // Act
            Action act = () => CommandMapperHelper.MapCardByCardType(_mapper, cardType, card);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Given_A_Spell_CardType_Should_Return_Object_Of_Type_SpellCardDto()
        {
            // Arrange
            const YgoCardType cardType = YgoCardType.Spell;

            var card = new Card { Id = 23424 };

            // Act
            var result = CommandMapperHelper.MapCardByCardType(_mapper, cardType, card);

            // Assert
            result.Should().BeOfType<SpellCardDto>();
        }

        [Test]
        public void Given_A_Trap_CardType_Should_Return_Object_Of_Type_TrapCardDto()
        {
            // Arrange
            const YgoCardType cardType = YgoCardType.Trap;

            var card = new Card { Id = 23424 };

            // Act
            var result = CommandMapperHelper.MapCardByCardType(_mapper, cardType, card);

            // Assert
            result.Should().BeOfType<TrapCardDto>();
        }


        [Test]
        public void Given_A_Monster_CardType_Should_Return_Object_Of_Type_TrapCardDto()
        {
            // Arrange
            const YgoCardType cardType = YgoCardType.Monster;

            var card = new Card { Id = 23424 };

            // Act
            var result = CommandMapperHelper.MapCardByCardType(_mapper, cardType, card);

            // Assert
            result.Should().BeOfType<MonsterCardDto>();
        }
    }
}
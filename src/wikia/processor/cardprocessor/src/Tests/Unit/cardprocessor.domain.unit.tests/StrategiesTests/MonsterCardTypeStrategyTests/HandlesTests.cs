using cardprocessor.core.Enums;
using cardprocessor.domain.Repository;
using cardprocessor.domain.Strategies;
using cardprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.StrategiesTests.MonsterCardTypeStrategyTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private ICardRepository _cardRepository;
        private MonsterCardTypeStrategy _sut;

        [SetUp]
        public void SetUp()
        {
            _cardRepository = Substitute.For<ICardRepository>();

            _sut = new MonsterCardTypeStrategy(_cardRepository);
        }

        [Test]
        public void Given_A_Monster_YugiohCardType_Should_Return_True()
        {
            // Arrange
            // Act
            var result = _sut.Handles(YugiohCardType.Monster);

            // Assert
            result.Should().BeTrue();
        }

        [TestCase(YugiohCardType.Trap)]
        [TestCase(YugiohCardType.Spell)]
        public void Given_An_Invalid_YugiohCardType_Should_Return_False(YugiohCardType yugiohCardType)
        {
            // Arrange
            // Act
            var result = _sut.Handles(yugiohCardType);

            // Assert
            result.Should().BeFalse();
        }
    }
}
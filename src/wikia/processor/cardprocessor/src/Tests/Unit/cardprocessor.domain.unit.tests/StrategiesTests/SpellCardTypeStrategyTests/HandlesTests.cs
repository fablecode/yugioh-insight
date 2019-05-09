using cardprocessor.core.Enums;
using cardprocessor.domain.Repository;
using cardprocessor.domain.Strategies;
using cardprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.domain.unit.tests.StrategiesTests.SpellCardTypeStrategyTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class HandlesTests
    {
        private ICardRepository _cardRepository;
        private SpellCardTypeStrategy _sut;

        [SetUp]
        public void SetUp()
        {
            _cardRepository = Substitute.For<ICardRepository>();

            _sut = new SpellCardTypeStrategy(_cardRepository);
        }

        [Test]
        public void Given_A_Spell_YugiohCardType_Should_Return_True()
        {
            // Arrange
            // Act
            var result = _sut.Handles(YugiohCardType.Spell);

            // Assert
            result.Should().BeTrue();
        }

        [TestCase(YugiohCardType.Monster)]
        [TestCase(YugiohCardType.Trap)]
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
using System;
using cardprocessor.application.Enums;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.application.Validations.Cards;
using cardprocessor.tests.core;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.ValidatorsTests.Commands
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AddCardCommandValidatorTests
    {
        private CardValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CardValidator();
        }


        [TestCase(-1)]
        [TestCase(3)]
        public void Given_An_Invalid_CardType_Validation_Should_Fail(YgoCardType cardType)
        {
            // Arrange
            var cardInputModel = new CardInputModel { CardType = cardType };

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.CardType, cardInputModel);

            // Assert
            act.Invoke();
        }

        [Test]
        public void Given_A_Null_CardType_Validation_Should_Fail()
        {
            // Arrange
            var cardInputModel = new CardInputModel();

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.CardType, cardInputModel);

            // Assert
            act.Invoke();
        }


    }
}
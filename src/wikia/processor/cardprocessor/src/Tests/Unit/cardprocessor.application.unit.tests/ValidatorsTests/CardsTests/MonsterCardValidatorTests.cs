using System;
using cardprocessor.application.Enums;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.application.Validations.Cards;
using cardprocessor.tests.core;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.ValidatorsTests.CardsTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MonsterCardValidatorTests
    {
        private MonsterCardValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new MonsterCardValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_An_Invalid_CardName_Validation_Should_Fail(string name)
        {
            // Arrange
            var inputModel = new CardInputModel{ CardType = YgoCardType.Monster, Name = name};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.Name, inputModel);

            // Assert
            act.Invoke();
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(21)]
        public void Given_An_Invalid_CardLevel_Validation_Should_Fail(int cardLevel)
        {
            // Arrange
            var inputModel = new CardInputModel{ CardType = YgoCardType.Monster, CardLevel = cardLevel};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.CardLevel, inputModel);

            // Assert
            act.Invoke();
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(21)]
        public void Given_An_Invalid_CardRank_Validation_Should_Fail(int cardRank)
        {
            // Arrange
            var inputModel = new CardInputModel{ CardType = YgoCardType.Monster, CardRank = cardRank};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.CardRank, inputModel);

            // Assert
            act.Invoke();
        }

        [TestCase(-1)]
        [TestCase(21000)]
        public void Given_An_Invalid_Ark_Validation_Should_Fail(int atk)
        {
            // Arrange
            var inputModel = new CardInputModel { CardType = YgoCardType.Monster, Atk = atk};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.Atk, inputModel);

            // Assert
            act.Invoke();
        }

        [TestCase(-1)]
        [TestCase(21000)]
        public void Given_An_Invalid_Def_Validation_Should_Fail(int def)
        {
            // Arrange
            var inputModel = new CardInputModel { CardType = YgoCardType.Monster, Def = def};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.Def, inputModel);

            // Assert
            act.Invoke();
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Given_An_Invalid_Attribute_Validation_Should_Fail(int attribute)
        {
            // Arrange
            var inputModel = new CardInputModel { CardType = YgoCardType.Monster, AttributeId = attribute};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.AttributeId, inputModel);

            // Assert
            act.Invoke();
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Given_An_Invalid_CardNumber_Validation_Should_Fail(int cardNumber)
        {
            // Arrange
            var inputModel = new CardInputModel { CardType = YgoCardType.Monster, CardNumber = cardNumber};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.CardNumber, inputModel);

            // Assert
            act.Invoke();
        }

        [Test]
        public void Given_An_Invalid_SubCategoryIds_Validation_Should_Fail()
        {
            // Arrange
            var inputModel = new CardInputModel { CardType = YgoCardType.Monster};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(c => c.SubCategoryIds, inputModel);

            // Assert
            act.Invoke();
        }
    }
}
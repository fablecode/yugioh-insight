using System;
using cardsectionprocessor.application.MessageConsumers.CardInformation;
using cardsectionprocessor.tests.core;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace cardsectionprocessor.application.unit.tests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardInformationConsumerValidatorTests
    {
        private CardInformationConsumerValidator _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CardInformationConsumerValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_An_Invalid_Category_Validation_Should_Fail(string category)
        {
            // Arrange
            var inputModel = new CardInformationConsumer { Category = category};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(ci => ci.Category, inputModel);

            // Assert
            act.Invoke();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Given_An_Invalid_Message_Validation_Should_Fail(string message)
        {
            // Arrange
            var inputModel = new CardInformationConsumer { Message = message};

            // Act
            Action act = () => _sut.ShouldHaveValidationErrorFor(ci => ci.Message, inputModel);

            // Assert
            act.Invoke();
        }
    }
}
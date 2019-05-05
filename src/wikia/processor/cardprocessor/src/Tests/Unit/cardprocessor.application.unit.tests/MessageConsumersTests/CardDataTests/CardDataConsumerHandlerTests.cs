using System;
using System.Threading;
using System.Threading.Tasks;
using cardprocessor.application.Commands;
using cardprocessor.application.Commands.AddCard;
using cardprocessor.application.Commands.UpdateCard;
using cardprocessor.application.MessageConsumers.CardData;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using cardprocessor.tests.core;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.MessageConsumersTests.CardDataTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardDataConsumerHandlerTests
    {
        private ICardService _cardService;
        private IMediator _mediator;
        private ICardCommandMapper _cardCommandMapper;
        private CardDataConsumerHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _cardService = Substitute.For<ICardService>();
            _mediator = Substitute.For<IMediator>();
            _cardCommandMapper = Substitute.For<ICardCommandMapper>();
            _sut = new CardDataConsumerHandler
            (
                _cardService,
                _mediator,
                _cardCommandMapper
            );
        }

        [Test]
        public async Task Given_CardData_Should_Execute_CardByName_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardDataConsumer = new CardDataConsumer
            {
                Message = "{\"Name\":\"Amazoness Archer\",\"Types\":\"Warrior / Effect\",\"CardType\":\"Monster\",\"Attribute\":\"Earth\",\"Level\":4,\"Rank\":null,\"PendulumScale\":null,\"AtkDef\":\"1400 / 1000\",\"AtkLink\":null,\"CardNumber\":\"91869203\",\"Materials\":null,\"CardEffectTypes\":\"Ignition\",\"Property\":null,\"Description\":\"You can Tribute 2 monsters; inflict 1200 damage to your opponent.\",\"ImageUrl\":\"https://vignette.wikia.nocookie.net/yugioh/images/0/02/AmazonessArcher-LEDU-EN-C-1E.png\",\"LinkArrows\":null,\"MonsterSubCategoriesAndTypes\":[\"Warrior\",\"Effect\"],\"MonsterLinkArrows\":null}"
            };

            _cardCommandMapper.MapToAddCommand(Arg.Any<YugiohCard>()).Returns(new AddCardCommand());
            _mediator.Send(Arg.Any<AddCardCommand>()).Returns(new CommandResult {IsSuccessful = true});

            // Act
            var result = await _sut.Handle(cardDataConsumer, CancellationToken.None);

            // Assert
            await _cardService.Received(expected).CardByName(Arg.Any<string>());
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_CardData_If_Exception_Occurrs_The_Response_Exception_Property_Should_Not_Be_Null()
        {
            // Arrange
            var cardDataConsumer = new CardDataConsumer
            {
                Message = "{\"Name\":\"Amazoness Archer\",\"Types\":\"Warrior / Effect\",\"CardType\":\"Monster\",\"Attribute\":\"Earth\",\"Level\":4,\"Rank\":null,\"PendulumScale\":null,\"AtkDef\":\"1400 / 1000\",\"AtkLink\":null,\"CardNumber\":\"91869203\",\"Materials\":null,\"CardEffectTypes\":\"Ignition\",\"Property\":null,\"Description\":\"You can Tribute 2 monsters; inflict 1200 damage to your opponent.\",\"ImageUrl\":\"https://vignette.wikia.nocookie.net/yugioh/images/0/02/AmazonessArcher-LEDU-EN-C-1E.png\",\"LinkArrows\":null,\"MonsterSubCategoriesAndTypes\":[\"Warrior\",\"Effect\"],\"MonsterLinkArrows\":null}"
            };

            _cardService.CardByName(Arg.Any<string>()).Throws(new Exception());

            // Act
            var result = await _sut.Handle(cardDataConsumer, CancellationToken.None);

            // Assert
            result.Exception.Should().NotBeNull();
        }

        [Test]
        public async Task Given_CardData_Should_Execute_MapToAddCommand_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardDataConsumer = new CardDataConsumer
            {
                Message = "{\"Name\":\"Amazoness Archer\",\"Types\":\"Warrior / Effect\",\"CardType\":\"Monster\",\"Attribute\":\"Earth\",\"Level\":4,\"Rank\":null,\"PendulumScale\":null,\"AtkDef\":\"1400 / 1000\",\"AtkLink\":null,\"CardNumber\":\"91869203\",\"Materials\":null,\"CardEffectTypes\":\"Ignition\",\"Property\":null,\"Description\":\"You can Tribute 2 monsters; inflict 1200 damage to your opponent.\",\"ImageUrl\":\"https://vignette.wikia.nocookie.net/yugioh/images/0/02/AmazonessArcher-LEDU-EN-C-1E.png\",\"LinkArrows\":null,\"MonsterSubCategoriesAndTypes\":[\"Warrior\",\"Effect\"],\"MonsterLinkArrows\":null}"
            };

            _cardCommandMapper.MapToAddCommand(Arg.Any<YugiohCard>()).Returns(new AddCardCommand());
            _mediator.Send(Arg.Any<AddCardCommand>()).Returns(new CommandResult { IsSuccessful = true });


            // Act
            var result = await _sut.Handle(cardDataConsumer, CancellationToken.None);

            // Assert
            await _cardCommandMapper.Received(expected).MapToAddCommand(Arg.Any<YugiohCard>());
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Given_CardData_Should_Execute_MapToUpdateCommand_Method_Once()
        {
            // Arrange
            const int expected = 1;
            var cardDataConsumer = new CardDataConsumer
            {
                Message = "{\"Name\":\"Amazoness Archer\",\"Types\":\"Warrior / Effect\",\"CardType\":\"Monster\",\"Attribute\":\"Earth\",\"Level\":4,\"Rank\":null,\"PendulumScale\":null,\"AtkDef\":\"1400 / 1000\",\"AtkLink\":null,\"CardNumber\":\"91869203\",\"Materials\":null,\"CardEffectTypes\":\"Ignition\",\"Property\":null,\"Description\":\"You can Tribute 2 monsters; inflict 1200 damage to your opponent.\",\"ImageUrl\":\"https://vignette.wikia.nocookie.net/yugioh/images/0/02/AmazonessArcher-LEDU-EN-C-1E.png\",\"LinkArrows\":null,\"MonsterSubCategoriesAndTypes\":[\"Warrior\",\"Effect\"],\"MonsterLinkArrows\":null}"
            };

            _cardService.CardByName(Arg.Any<string>()).Returns(new Card());
            _cardCommandMapper.MapToUpdateCommand(Arg.Any<YugiohCard>(), Arg.Any<Card>()).Returns(new UpdateCardCommand());
            _mediator.Send(Arg.Any<UpdateCardCommand>()).Returns(new CommandResult { IsSuccessful = true });


            // Act
            var result = await _sut.Handle(cardDataConsumer, CancellationToken.None);

            // Assert
            await _cardCommandMapper.Received(expected).MapToUpdateCommand(Arg.Any<YugiohCard>(), Arg.Any<Card>());
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
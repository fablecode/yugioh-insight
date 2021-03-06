﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using cardprocessor.application.Commands.DownloadImage;
using cardprocessor.application.Commands.UpdateCard;
using cardprocessor.application.Configuration;
using cardprocessor.application.Enums;
using cardprocessor.application.Mappings.Profiles;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.application.Validations.Cards;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using cardprocessor.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.Commands
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class UpdateCardCommandHandlerTests
    {
        private UpdateCardCommandHandler _sut;
        private IMediator _mediator;
        private ICardService _cardService;
        private IOptions<AppSettings> _settings;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _settings = Substitute.For<IOptions<AppSettings>>();

            var config = new MapperConfiguration
            (
                cfg => { cfg.AddProfile(new CardProfile()); }
            );

            var mapper = config.CreateMapper();

            _cardService = Substitute.For<ICardService>();
            _sut = new UpdateCardCommandHandler(_mediator, new CardValidator(), _cardService, _settings, mapper);
        }

        [Test]
        public async Task Given_An_Invalid_UpdateCardCommand_Validation_Should_Fail()
        {
            // Arrange
            var command = new UpdateCardCommand { Card = new CardInputModel() };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Given_An_Invalid_UpdateCardCommand_Should_Return_A_List_Of_Errors()
        {
            // Arrange
            var command = new UpdateCardCommand { Card = new CardInputModel() };

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_A_Valid_Card_And_UpdateCardCommand_Fails_Should_Return_Error_List()
        {
            // Arrange
            var command = new UpdateCardCommand
            {
                Card = new CardInputModel
                {
                    CardType = YgoCardType.Spell,
                    Name = "Monster Reborn",
                    Description = "Special Summon a monster from any graveyard"
                }
            };

            _cardService.Update(Arg.Any<CardModel>()).Returns((Card)null);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_A_Valid_Card_With_An_ImageUrl_Should_Invoke_DownloadImageCommand()
        {
            // Arrange
            var command = new UpdateCardCommand
            {
                Card = new CardInputModel
                {
                    CardType = YgoCardType.Spell,
                    Name = "Monster Reborn",
                    Description = "Special Summon a monster from any graveyard",
                    ImageUrl = new Uri("http://cardimageurl.com/card/image.jpg")
                }
            };

            _cardService.Update(Arg.Any<CardModel>()).Returns(new Card { Id = 23424 });
            _settings.Value.Returns(new AppSettings
            {
                CardImageFolderPath = "C:/cards/images"
            });

            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<DownloadImageCommand>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Given_A_Valid_Card_Should_Invoke_CardService_Update_Method_Once()
        {
            // Arrange
            var command = new UpdateCardCommand
            {
                Card = new CardInputModel
                {
                    CardType = YgoCardType.Trap,
                    Name = "Call of the Haunted",
                    Description = "Activate this card by targeting 1 monster in your GY; Special Summon that target in Attack Position. When this card leaves the field, destroy that target. When that target is destroyed, destroy this card",
                    ImageUrl = new Uri("http://cardimageurl.com/card/image.jpg")
                }
            };

            _cardService.Update(Arg.Any<CardModel>()).Returns(new Card { Id = 23424 });
            _settings.Value.Returns(new AppSettings
            {
                CardImageFolderPath = "C:/cards/images"
            });


            // Act
            await _sut.Handle(command, CancellationToken.None);

            // Assert
            await _cardService.Received(1).Update(Arg.Any<CardModel>());
        }

        [Test]
        public async Task Given_A_Valid_Card_ISuccessful_Should_Be_True()
        {
            // Arrange
            var command = new UpdateCardCommand
            {
                Card = new CardInputModel
                {
                    CardType = YgoCardType.Spell,
                    Name = "Monster Reborn",
                    Description = "Special Summon a monster from any graveyard",
                    ImageUrl = new Uri("http://cardimageurl.com/card/image.jpg")
                }
            };

            _cardService.Update(Arg.Any<CardModel>()).Returns(new Card { Id = 23424 });
            _settings.Value.Returns(new AppSettings
            {
                CardImageFolderPath = "C:/cards/images"
            });


            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
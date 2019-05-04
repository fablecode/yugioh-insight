using cardprocessor.application.Commands;
using cardprocessor.application.Commands.AddCard;
using cardprocessor.application.Commands.UpdateCard;
using cardprocessor.application.Enums;
using cardprocessor.application.Helpers.Cards;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attribute = cardprocessor.core.Models.Db.Attribute;
using Type = cardprocessor.core.Models.Db.Type;

namespace cardprocessor.application.Mappings.Mappers
{
    public class CardCommandMapper : ICardCommandMapper
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ITypeService _typeService;
        private readonly IAttributeService _attributeService;
        private readonly ILinkArrowService _linkArrowService;

        public CardCommandMapper
        (
            ICategoryService categoryService, 
            ISubCategoryService subCategoryService, 
            ITypeService typeService, 
            IAttributeService attributeService, 
            ILinkArrowService linkArrowService
        )
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _typeService = typeService;
            _attributeService = attributeService;
            _linkArrowService = linkArrowService;
        }


        public async Task<AddCardCommand> MapToAddCommand(YugiohCard yugiohCard)
        {
            ICollection<Category> categories = await _categoryService.AllCategories();
            ICollection<SubCategory> subCategories = await _subCategoryService.AllSubCategories();

            var command = new AddCardCommand
            {
                Card = new CardInputModel()
            };

            CardHelper.MapBasicCardInformation(yugiohCard, command.Card);
            CardHelper.MapCardImageUrl(yugiohCard, command.Card);

            if (command.Card.CardType.Equals(YgoCardType.Spell))
            {
                SpellCardHelper.MapSubCategoryIds(yugiohCard, command.Card, categories, subCategories);
            }
            else if (command.Card.CardType.Equals(YgoCardType.Trap))
            {
                TrapCardHelper.MapSubCategoryIds(yugiohCard, command.Card, categories, subCategories);
            }
            else
            {
                ICollection<Type> types = await _typeService.AllTypes();
                ICollection<Attribute> attributes = await _attributeService.AllAttributes();
                ICollection<LinkArrow> linkArrows = await _linkArrowService.AllLinkArrows();

                var monsterCategory = categories.Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
                var monsterSubCategories = subCategories.Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

                MonsterCardHelper.MapMonsterCard(yugiohCard, command.Card, attributes, monsterSubCategories, types, linkArrows);
            }

            return command;
        }


        public async Task<UpdateCardCommand> MapToUpdateCommand(YugiohCard yugiohCard, Card cardToUpdate)
        {
            ICollection<Category> categories = await _categoryService.AllCategories();
            ICollection<SubCategory> subCategories = await _subCategoryService.AllSubCategories();

            var command = new UpdateCardCommand
            {
                Card = new CardInputModel()
            };

            command.Card.Id = cardToUpdate.Id;
            command.Card.CardType = (YgoCardType?)(Enum.TryParse(typeof(YgoCardType), yugiohCard.CardType, true, out var cardType) ? cardType : null);
            command.Card.CardNumber = long.TryParse(yugiohCard.CardNumber, out var cardNumber) ? cardNumber : (long?)null;
            command.Card.Name = yugiohCard.Name;
            command.Card.Description = yugiohCard.Description;

            if (yugiohCard.ImageUrl != null)
                command.Card.ImageUrl = new Uri(yugiohCard.ImageUrl);

            if (command.Card.CardType.Equals(YgoCardType.Spell))
            {
                command.Card.SubCategoryIds = new List<int>
                {
                    SubCategoryMapper.SubCategoryId(categories, subCategories, YgoCardType.Spell, yugiohCard.Property)
                };
            }
            else if (command.Card.CardType.Equals(YgoCardType.Trap))
            {
                command.Card.SubCategoryIds = new List<int>
                {
                    SubCategoryMapper.SubCategoryId(categories, subCategories, YgoCardType.Trap, yugiohCard.Property)
                };
            }
            else
            {
                ICollection<Type> types = await _typeService.AllTypes();
                ICollection<Attribute> attributes = await _attributeService.AllAttributes();
                ICollection<LinkArrow> linkArrows = await _linkArrowService.AllLinkArrows();

                var monsterCategory = categories.Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
                var monsterSubCategories = subCategories.Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

                command.Card.AttributeId = MonsterCardHelper.MonsterAttributeId(yugiohCard, attributes);

                command.Card.SubCategoryIds = MonsterCardHelper.MonsterSubCategoryIds(yugiohCard, monsterSubCategories);

                command.Card.TypeIds = MonsterCardHelper.MonsterTypeIds(yugiohCard, types);

                if (yugiohCard.LinkArrows != null)
                {
                    command.Card.LinkArrowIds = MonsterCardHelper.MonsterLinkArrowIds(yugiohCard, linkArrows);
                }

                if (yugiohCard.Level.HasValue)
                    command.Card.CardLevel = yugiohCard.Level;

                if(yugiohCard.Rank.HasValue)
                    command.Card.CardRank = yugiohCard.Rank;

                if (!string.IsNullOrWhiteSpace(yugiohCard.AtkDef))
                {
                    var atk = MonsterCardHelper.Atk(yugiohCard);
                    var def = MonsterCardHelper.DefOrLink(yugiohCard);

                    int.TryParse(atk, out var cardAtk);
                    int.TryParse(def, out var cardDef);

                    command.Card.Atk = cardAtk;
                    command.Card.Def = cardDef;
                }

                if (!string.IsNullOrWhiteSpace(yugiohCard.AtkLink))
                {
                    var atk = MonsterCardHelper.AtkLink(yugiohCard);

                    int.TryParse(atk, out var cardAtk);

                    command.Card.Atk = cardAtk;
                }
            }

            return command;
        }
    }
}
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
                Card = await MapToCardInputModel(yugiohCard, new CardInputModel(), categories, subCategories)
            };

            return command;
        }


        public async Task<UpdateCardCommand> MapToUpdateCommand(YugiohCard yugiohCard, Card cardToUpdate)
        {
            ICollection<Category> categories = await _categoryService.AllCategories();
            ICollection<SubCategory> subCategories = await _subCategoryService.AllSubCategories();

            var command = new UpdateCardCommand
            {
                Card = await MapToCardInputModel(yugiohCard, new CardInputModel(), categories, subCategories, cardToUpdate)
            };

            return command;
        }

        #region private helpers

        private async Task<CardInputModel> MapToCardInputModel(YugiohCard yugiohCard, CardInputModel cardInputModel, ICollection<Category> categories, ICollection<SubCategory> subCategories, Card cardToUpdate)
        {
            cardInputModel.Id = cardToUpdate.Id;
            return await MapToCardInputModel(yugiohCard, cardInputModel, categories, subCategories);
        }

        private async Task<CardInputModel> MapToCardInputModel(YugiohCard yugiohCard, CardInputModel cardInputModel, ICollection<Category> categories, ICollection<SubCategory> subCategories)
        {
            CardHelper.MapBasicCardInformation(yugiohCard, cardInputModel);
            CardHelper.MapCardImageUrl(yugiohCard, cardInputModel);

            if (cardInputModel.CardType.Equals(YgoCardType.Spell))
            {
                SpellCardHelper.MapSubCategoryIds(yugiohCard, cardInputModel, categories, subCategories);
            }
            else if (cardInputModel.CardType.Equals(YgoCardType.Trap))
            {
                TrapCardHelper.MapSubCategoryIds(yugiohCard, cardInputModel, categories, subCategories);
            }
            else
            {
                ICollection<Type> types = await _typeService.AllTypes();
                ICollection<Attribute> attributes = await _attributeService.AllAttributes();
                ICollection<LinkArrow> linkArrows = await _linkArrowService.AllLinkArrows();

                var monsterCategory = categories.Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
                var monsterSubCategories = subCategories.Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

                MonsterCardHelper.MapMonsterCard(yugiohCard, cardInputModel, attributes, monsterSubCategories, types, linkArrows);
            }

            return cardInputModel;
        }

        #endregion
    }
}
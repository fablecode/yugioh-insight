using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cardprocessor.application.Commands.AddCard;
using cardprocessor.application.Commands.UpdateCard;
using cardprocessor.application.Enums;
using cardprocessor.application.Models.Cards.Input;
using cardprocessor.core.Models;
using cardprocessor.core.Models.Db;
using cardprocessor.core.Services;
using Attribute = cardprocessor.core.Models.Db.Attribute;
using Type = cardprocessor.core.Models.Db.Type;

namespace cardprocessor.application.Commands
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

            command.Card.CardType = (YgoCardType?) (Enum.TryParse(typeof(YgoCardType), yugiohCard.CardType, true, out var cardType) ? cardType : null);
            command.Card.CardNumber = long.TryParse(yugiohCard.CardNumber, out var cardNumber) ? cardNumber : (long?) null;
            command.Card.Name = yugiohCard.Name;
            command.Card.Description = yugiohCard.Description;

            if (yugiohCard.ImageUrl != null)
                command.Card.ImageUrl = new Uri(yugiohCard.ImageUrl);


            if (command.Card.CardType.Equals(YgoCardType.Spell))
            {
                command.Card.SubCategoryIds = new List<int>
                {
                    SubCategoryId(categories, subCategories, YgoCardType.Spell, yugiohCard.Property)
                };
            }
            else if (command.Card.CardType.Equals(YgoCardType.Trap))
            {
                command.Card.SubCategoryIds = new List<int>
                {
                    SubCategoryId(categories, subCategories, YgoCardType.Trap, yugiohCard.Property)
                };
            }
            else
            {
                ICollection<Type> types = await _typeService.AllTypes();
                ICollection<Attribute> attributes = await _attributeService.AllAttributes();
                ICollection<LinkArrow> linkArrows = await _linkArrowService.AllLinkArrows();

                var monsterCategory = categories.Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
                var monsterSubCategories = subCategories.Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

                command.Card.AttributeId = MonsterAttributeId(yugiohCard, attributes);

                command.Card.SubCategoryIds = MonsterSubCategoryIds(yugiohCard, monsterSubCategories);
                command.Card.TypeIds = MonsterTypeIds(yugiohCard, types);

                if (yugiohCard.LinkArrows != null)
                {
                    command.Card.LinkArrowIds = MonsterLinkArrowIds(yugiohCard, linkArrows);
                }


                if (yugiohCard.Level.HasValue)
                    command.Card.CardLevel = yugiohCard.Level;

                if (yugiohCard.Rank.HasValue)
                    command.Card.CardRank = yugiohCard.Rank;

                if (!string.IsNullOrWhiteSpace(yugiohCard.AtkDef))
                {
                    var atk = Atk(yugiohCard);
                    var def = DefOrLink(yugiohCard);

                    int.TryParse(atk, out var cardAtk);
                    int.TryParse(def, out var cardDef);

                    command.Card.Atk = cardAtk;
                    command.Card.Def = cardDef;
                }

                if (!string.IsNullOrWhiteSpace(yugiohCard.AtkLink))
                {
                    var atk = Atk(yugiohCard);

                    int.TryParse(atk, out var cardAtk);

                    command.Card.Atk = cardAtk;
                }
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
                    SubCategoryId(categories, subCategories, YgoCardType.Spell, yugiohCard.Property)
                };
            }
            else if (command.Card.CardType.Equals(YgoCardType.Trap))
            {
                command.Card.SubCategoryIds = new List<int>
                {
                    SubCategoryId(categories, subCategories, YgoCardType.Trap, yugiohCard.Property)
                };
            }
            else
            {
                ICollection<Type> types = await _typeService.AllTypes();
                ICollection<Attribute> attributes = await _attributeService.AllAttributes();
                ICollection<LinkArrow> linkArrows = await _linkArrowService.AllLinkArrows();

                var monsterCategory = categories.Single(c => c.Name.Equals(YgoCardType.Monster.ToString(), StringComparison.OrdinalIgnoreCase));
                var monsterSubCategories = subCategories.Select(sc => sc).Where(sc => sc.CategoryId == monsterCategory.Id);

                command.Card.AttributeId = MonsterAttributeId(yugiohCard, attributes);

                command.Card.SubCategoryIds = MonsterSubCategoryIds(yugiohCard, monsterSubCategories);

                command.Card.TypeIds = MonsterTypeIds(yugiohCard, types);

                if (yugiohCard.LinkArrows != null)
                {
                    command.Card.LinkArrowIds = MonsterLinkArrowIds(yugiohCard, linkArrows);
                }

                if (yugiohCard.Level.HasValue)
                    command.Card.CardLevel = yugiohCard.Level;

                if(yugiohCard.Rank.HasValue)
                    command.Card.CardRank = yugiohCard.Rank;

                if (!string.IsNullOrWhiteSpace(yugiohCard.AtkDef))
                {
                    var atk = Atk(yugiohCard);
                    var def = DefOrLink(yugiohCard);

                    int.TryParse(atk, out var cardAtk);
                    int.TryParse(def, out var cardDef);

                    command.Card.Atk = cardAtk;
                    command.Card.Def = cardDef;
                }

                if (!string.IsNullOrWhiteSpace(yugiohCard.AtkLink))
                {
                    var atk = Atk(yugiohCard);

                    int.TryParse(atk, out var cardAtk);

                    command.Card.Atk = cardAtk;
                }
            }

            return command;
        }

        #region private helpers

        private static int SubCategoryId(IEnumerable<Category> categories, IEnumerable<SubCategory> subCategories, YgoCardType cardType, string subCategory)
        {
            return
                (int)
                (
                    from sc in subCategories
                    join c in categories on sc.CategoryId equals c.Id
                    where c.Name.Equals(cardType.ToString(), StringComparison.OrdinalIgnoreCase) && sc.Name.Equals(subCategory, StringComparison.OrdinalIgnoreCase)
                    select sc.Id
                )
                .Single();
        }

        private static List<int> MonsterLinkArrowIds(YugiohCard yugiohCard, ICollection<LinkArrow> linkArrows)
        {
            return linkArrows
                .Where(mla => yugiohCard.MonsterLinkArrows.Any(ymla => ymla.Equals(mla.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(la => (int)la.Id)
                .ToList();
        }

        private static List<int> MonsterTypeIds(YugiohCard yugiohCard, ICollection<Type> types)
        {
            return types
                .Where(mt => yugiohCard.MonsterSubCategoriesAndTypes.Any(yt => yt.Equals(mt.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(smt => (int)smt.Id)
                .ToList();
        }

        private static List<int> MonsterSubCategoryIds(YugiohCard yugiohCard, IEnumerable<SubCategory> monsterSubCategories)
        {
            return monsterSubCategories
                .Where(msb => yugiohCard.MonsterSubCategoriesAndTypes.Any(ysc => ysc.Equals(msb.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(ssc => (int)ssc.Id)
                .ToList();
        }

        private static int MonsterAttributeId(YugiohCard yugiohCard, ICollection<Attribute> attributes)
        {
            return
                (int) 
                (
                    from a in attributes
                    where a.Name.Equals(yugiohCard.Attribute, StringComparison.OrdinalIgnoreCase)
                    select a.Id
                )
                .Single();
        }

        private static string Atk(YugiohCard yugiohCard)
        {
            return yugiohCard.AtkDef?.Split('/').First();
        }

        private static string DefOrLink(YugiohCard yugiohCard)
        {
            return yugiohCard.AtkDef?.Split('/').Last();
        }

        #endregion
    }
}
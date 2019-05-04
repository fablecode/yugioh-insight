using System.Collections.Generic;
using System.Threading.Tasks;
using cardprocessor.application.Enums;
using cardprocessor.application.Mappings.Mappers;
using cardprocessor.core.Models;
using cardprocessor.core.Services;
using cardprocessor.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace cardprocessor.application.unit.tests.MappingsTests.MapperTests.CardCommandMapperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MapToAddCommandTests
    {
        private ICategoryService _categoryService;
        private ISubCategoryService _subCategoryService;
        private ITypeService _typeService;
        private IAttributeService _attributeService;
        private ILinkArrowService _linkArrowService;
        private CardCommandMapper _sut;

        [SetUp]
        public void SetUp()
        {
            _categoryService = Substitute.For<ICategoryService>();
            _subCategoryService = Substitute.For<ISubCategoryService>();
            _typeService = Substitute.For<ITypeService>();
            _attributeService = Substitute.For<IAttributeService>();
            _linkArrowService = Substitute.For<ILinkArrowService>();

            _categoryService.AllCategories().Returns(TestData.AllCategories());
            _subCategoryService.AllSubCategories().Returns(TestData.AllSubCategories());
            _typeService.AllTypes().Returns(TestData.AllTypes());
            _attributeService.AllAttributes().Returns(TestData.AllAttributes());
            _linkArrowService.AllLinkArrows().Returns(TestData.AllLinkArrows());

            _sut = new CardCommandMapper
            (
                _categoryService,
                _subCategoryService,
                _typeService,
                _attributeService,
                _linkArrowService
            );
        }

        [Test]
        public void Given_A_Valid_Spell_Type_YugiohCard_Should_Return_AddCommand()
        {
            // Arrange
            // Act
            var result = _sut.MapToAddCommand(new YugiohCard());

            // Assert
            result.Should().NotBeNull();
        }

        [TestCase("Spell", YgoCardType.Spell)]
        [TestCase("Trap", YgoCardType.Trap)]
        [TestCase("speLl", YgoCardType.Spell)]
        [TestCase("traP", YgoCardType.Trap)]
        public async Task Given_A_Valid_SpellOrTrap_CardType_Should_Map_CardType_To_Correct_YgoCardType(string cardType, YgoCardType expected)
        {
            // Arrange

            // Act
            var result = await _sut.MapToAddCommand(new YugiohCard
            {
                Property = "Normal",
                CardType = cardType
            });

            // Assert
            result.Card.CardType.Should().Be(expected);
        }

        [TestCase("Monster", YgoCardType.Monster)]
        [TestCase("monsTer", YgoCardType.Monster)]
        public async Task Given_A_Valid_Monster_CardType_Should_Map_CardType_To_Correct_YgoCardType(string cardType, YgoCardType expected)
        {
            // Arrange
            var yugiohCard = new YugiohCard
            {
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = cardType
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.CardType.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_A_CardNumber_Should_Map_To_CardNumber_Property()
        {
            // Arrange
            const long expected = 543534;

            var yugiohCard = new YugiohCard
            {
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.CardNumber.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_A_Name_Should_Map_To_Name_Property()
        {
            // Arrange
            const string expected = "Darkness Dragon";

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.Name.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_A_Description_Should_Map_To_Description_Property()
        {
            // Arrange
            const string expected = "Amazing card!";

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.Description.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_An_ImageUrl_Should_Map_To_ImageUrl_Property()
        {
            // Arrange
            const string expected = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125";

            var yugiohCard = new YugiohCard
            {
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.ImageUrl.AbsoluteUri.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_Types_Should_Map_To_SubCategoryIds_Property()
        {
            // Arrange
             var expected = new List<int>{2, 4};

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.SubCategoryIds.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_Types_Should_Map_To_TypeIds_Property()
        {
            // Arrange
             var expected = new List<int>{8};

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.TypeIds.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_LinkArrows_Should_Map_To_LinkArrowIds_Property()
        {
            // Arrange
             var expected = new List<int>{5, 7};

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                LinkArrows = "Bottom-Left,Bottom-Right",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.LinkArrowIds.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_A_Level_Should_Map_To_CardLevel_Property()
        {
            // Arrange
             const int expected = 8;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Level = 8,
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.CardLevel.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_A_Rank_Should_Map_To_CardRank_Property()
        {
            // Arrange
             const int expected = 3;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Rank = 3,
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.CardRank.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_An_Atk_Should_Map_To_Atk_Property()
        {
            // Arrange
             const int expected = 3000;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Level = 8,
                AtkDef = "3000 / 2500",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.Atk.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_Link_Monster_YugiohCard_With_An_Atk_Should_Map_To_Atk_Property()
        {
            // Arrange
             const int expected = 2300;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Rank = 3,
                AtkLink = "2300 / 3",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.Atk.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_Valid_YugiohCard_With_An_Def_Should_Map_To_Def_Property()
        {
            // Arrange
             const int expected = 2500;

            var yugiohCard = new YugiohCard
            {
                Name = "Darkness Dragon",
                Description = "Amazing card!",
                CardNumber = "543534",
                Attribute = "Dark",
                Types = "Effect/Ritual/Dragon",
                CardType = "Monster",
                Level = 8,
                AtkDef = "3000 / 2500",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/b/b4/BlueEyesWhiteDragon-LED3-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180928161125"
            };

            // Act
            var result = await _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Card.Def.Should().Be(expected);
        }
    }
}
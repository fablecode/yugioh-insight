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
            var yugiohCard = new YugiohCard
            {
                Name = "Black Illusion Ritual",

                Description = "Amazing card!",
                CardNumber = "41426869",
                Property = "Ritual",
                CardType = "Spell",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/8/82/BlackIllusionRitual-LED2-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180223153750"
            };

            // Act
            var result = _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Given_A_Valid_Trap_Type_YugiohCard_Should_Return_AddCommand()
        {
            // Arrange
            var yugiohCard = new YugiohCard
            {
                Name = "Call of the Haunted",
                Description = "Amazing card!",
                CardNumber = "97077563",
                Property = "Continuous",
                CardType = "Trap",
                ImageUrl = "https://vignette.wikia.nocookie.net/yugioh/images/4/47/CalloftheHaunted-YS18-EN-C-1E.png/revision/latest/scale-to-width-down/300?cb=20180712163539"
            };

            // Act
            var result = _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Given_A_Valid_Monster_Type_YugiohCard_Should_Return_AddCommand()
        {
            // Arrange
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
            var result = _sut.MapToAddCommand(yugiohCard);

            // Assert
            result.Should().NotBeNull();
        }

    }
}
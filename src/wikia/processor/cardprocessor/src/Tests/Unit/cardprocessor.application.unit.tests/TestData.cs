using System.Collections.Generic;
using System.Linq;
using cardprocessor.core.Models.Db;

namespace cardprocessor.application.unit.tests
{
    public static class TestData
    {
        public static List<Category> AllCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Monster"
                },
                new Category
                {
                    Id = 2,
                    Name = "Spell"
                },
                new Category
                {
                    Id = 3,
                    Name = "Trap"
                }
            };
        }
        public static List<SubCategory> AllSubCategories()
        {
            var monster = AllCategories().Single(c => c.Name == "Monster");
            var spell = AllCategories().Single(c => c.Name == "Spell");
            var trap = AllCategories().Single(c => c.Name == "Trap");

            return new List<SubCategory>
            {
                new SubCategory
                {
                    Id = 1,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Normal"
                },
                new SubCategory
                {
                    Id = 2,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Effect"
                },
                new SubCategory
                {
                    Id = 3,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Fusion"
                },
                new SubCategory
                {
                    Id = 4,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Ritual"
                },
                new SubCategory
                {
                    Id = 5,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Synchro"
                },
                new SubCategory
                {
                    Id = 6,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Xyz"
                },
                new SubCategory
                {
                    Id = 7,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Pendulum"
                },
                new SubCategory
                {
                    Id = 8,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Tuner"
                },
                new SubCategory
                {
                    Id = 9,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Gemini"
                },
                new SubCategory
                {
                    Id = 10,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Union"
                },
                new SubCategory
                {
                    Id = 11,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Spirit"
                },
                new SubCategory
                {
                    Id = 12,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Flip"
                },
                new SubCategory
                {
                    Id = 13,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Toon"
                },
                new SubCategory
                {
                    Id = 14,
                    CategoryId = 1,
                    Category = monster,
                    Name = "Link"
                },
                new SubCategory
                {
                    Id = 15,
                    CategoryId = 2,
                    Category = spell,
                    Name = "Normal"
                },
                new SubCategory
                {
                    Id = 16,
                    CategoryId = 2,
                    Category = spell,
                    Name = "Quick-Play"
                },
                new SubCategory
                {
                    Id = 17,
                    CategoryId = 2,
                    Category = spell,
                    Name = "Continuous"
                },
                new SubCategory
                {
                    Id = 18,
                    CategoryId = 2,
                    Category = spell,
                    Name = "Ritual"
                },
                new SubCategory
                {
                    Id = 19,
                    CategoryId = 2,
                    Category = spell,
                    Name = "Equip"
                },
                new SubCategory
                {
                    Id = 20,
                    CategoryId = 2,
                    Category = spell,
                    Name = "Field"
                },
                new SubCategory
                {
                    Id = 21,
                    CategoryId = 3,
                    Category = trap,
                    Name = "Normal"
                },
                new SubCategory
                {
                    Id = 22,
                    CategoryId = 3,
                    Category = trap,
                    Name = "Continuous"
                },
                new SubCategory
                {
                    Id = 23,
                    CategoryId = 3,
                    Category = trap,
                    Name = "Counter"
                }
            };
        }

        public static List<Type> AllTypes()
        {
            return new List<Type>
            {
                new Type
                {
                    Id = 1,
                    Name = "Aqua"
                },
                new Type
                {
                    Id = 2,
                    Name = "Beast"
                },
                new Type
                {
                    Id = 3,
                    Name = "Beast-Warrior"
                },
                new Type
                {
                    Id = 4,
                    Name = "Creator God"
                },
                new Type
                {
                    Id = 5,
                    Name = "Cyberse"
                },
                new Type
                {
                    Id = 6,
                    Name = "Dinosaur"
                },
                new Type
                {
                    Id = 7,
                    Name = "Divine-Beast"
                },
                new Type
                {
                    Id = 8,
                    Name = "Dragon"
                },
                new Type
                {
                    Id = 9,
                    Name = "Fairy"
                },
                new Type
                {
                    Id = 10,
                    Name = "Fiend"
                },
                new Type
                {
                    Id = 11,
                    Name = "Fish"
                },
                new Type
                {
                    Id = 12,
                    Name = "Insect"
                },
                new Type
                {
                    Id = 13,
                    Name = "Machine"
                },
                new Type
                {
                    Id = 14,
                    Name = "Plant"
                },
                new Type
                {
                    Id = 15,
                    Name = "Psychic"
                },
                new Type
                {
                    Id = 16,
                    Name = "Pyro"
                },
                new Type
                {
                    Id = 17,
                    Name = "Reptile"
                },
                new Type
                {
                    Id = 18,
                    Name = "Rock"
                },
                new Type
                {
                    Id = 19,
                    Name = "Sea Serpent"
                },
                new Type
                {
                    Id = 20,
                    Name = "Spellcaster"
                },
                new Type
                {
                    Id = 21,
                    Name = "Thunder"
                },
                new Type
                {
                    Id = 22,
                    Name = "Warrior"
                },
                new Type
                {
                    Id = 23,
                    Name = "Winged Beast"
                },
                new Type
                {
                    Id = 24,
                    Name = "Wyrm"
                },
                new Type
                {
                    Id = 25,
                    Name = "Zombie"
                },
            };
        }

        public static List<Attribute> AllAttributes()
        {
            return new List<Attribute>
            {
                new Attribute
                {
                    Id = 1,
                    Name = "Dark"
                },
                new Attribute
                {
                    Id = 2,
                    Name = "Divine"
                },
                new Attribute
                {
                    Id = 3,
                    Name = "Earth"
                },
                new Attribute
                {
                    Id = 4,
                    Name = "Fire"
                },
                new Attribute
                {
                    Id = 5,
                    Name = "Light"
                },
                new Attribute
                {
                    Id = 6,
                    Name = "Water"
                },
                new Attribute
                {
                    Id = 7,
                    Name = "Wind"
                }
            };
        }

        public static List<LinkArrow> AllLinkArrows()
        {
            return new List<LinkArrow>
            {
                new LinkArrow
                {
                    Id = 1,
                    Name = "Top-Left"
                },
                new LinkArrow
                {
                    Id = 2,
                    Name = "Top"
                },
                new LinkArrow
                {
                    Id = 3,
                    Name = "Top-Right"
                },
                new LinkArrow
                {
                    Id = 4,
                    Name = "Right"
                },
                new LinkArrow
                {
                    Id = 5,
                    Name = "Bottom-Right"
                },
                new LinkArrow
                {
                    Id = 6,
                    Name = "Bottom"
                },
                new LinkArrow
                {
                    Id = 7,
                    Name = "Bottom-Left"
                },
                new LinkArrow
                {
                    Id = 8,
                    Name = "Left"
                }
            };
        }
    }
}
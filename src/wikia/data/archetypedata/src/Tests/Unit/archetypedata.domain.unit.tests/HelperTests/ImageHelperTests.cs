using archetypedata.domain.Helpers;
using archetypedata.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace archetypedata.domain.unit.tests.HelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ImageHelperTests
    {
        [TestCase("https://vignette.wikia.nocookie.net/yugioh/images/6/65/EvolzarLaggia-TF06-JP-VG.png/revision/latest/window-crop/width/200/x-offset/0/y-offset/0/window-width/545/window-height/544?cb=20110928032728", "https://vignette.wikia.nocookie.net/yugioh/images/6/65/EvolzarLaggia-TF06-JP-VG.png")]
        [TestCase("https://vignette.wikia.nocookie.net/yugioh/images/c/c2/AethertheEmpoweringDragon-YS14-EN-C-1E.png/revision/latest/window-crop/width/200/x-offset/0/y-offset/0/window-width/308/window-height/308?cb=20140711051108", "https://vignette.wikia.nocookie.net/yugioh/images/c/c2/AethertheEmpoweringDragon-YS14-EN-C-1E.png")]
        [TestCase("", null)]
        public void Given_An_Image_Url_Should_Extract_Url_Without_Image_Sizes(string url, string expect)
        {
            // Arrange

            // Act
            var result = ImageHelper.ExtractImageUrl(url);

            // Assert
            result.Should().BeEquivalentTo(expect);
        }
    }
}
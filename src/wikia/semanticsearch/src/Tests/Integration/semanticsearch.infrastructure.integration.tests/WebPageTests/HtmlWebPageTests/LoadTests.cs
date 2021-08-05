using FluentAssertions;
using NUnit.Framework;
using semanticsearch.infrastructure.WebPage;
using semanticsearch.tests.core;

namespace semanticsearch.infrastructure.integration.tests.WebPageTests.HtmlWebPageTests
{
    [TestFixture]
    [Category(TestType.Integration)]
    public class LoadTests
    {
        private HtmlWebPage _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new HtmlWebPage();
        }

        [Test]
        public void Given_A_WebPage_Url_Should_Load_WebPage_To_HtmlDocument()
        {
            // Arrange
            const string url = "https://www.google.com/";

            // Act
            var result = _sut.Load(url);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
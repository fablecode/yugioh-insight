using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using semanticsearch.infrastructure.WebPage;

namespace semanticsearch.infrastructure.integration.tests.WebPageTests.SemanticCardSearchResultsWebPageTests
{
    [TestFixture]
    public class NameTests
    {
        private SemanticCardSearchResultsWebPage _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SemanticCardSearchResultsWebPage();
        }

        [Test]
        public void Given_A_Html_Table_Row_Should_Extract_The_First_Card_Name_Which_Is_MegasonicEye()
        {
            // Arrange
            const string expected = "Megasonic Eye";
            const string url = "https://yugioh.fandom.com/wiki/Special:Ask?limit=500&offset=500&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D&p=format%3Dbroadtable%2Flink%3Dall%2Fheaders%3Dshow%2Fsearchlabel%3D%E2%80%A6-20further-20results%2Fclass%3D-20sortable-20wikitable-20smwtable-20card-2Dlist&po=%3FJapanese+name%0A%3FRank%0A%3FLevel%0A%3FAttribute%0A%3FType%0A%3FMonster+type%0A%3FATK+string%3DATK%0A%3FDEF+string%3DDEF%0A&sort=&order=&eq=yes#search";
            var semanticSearchResultsWebPage = new SemanticSearchResultsWebPage(new HtmlWebPage());
            semanticSearchResultsWebPage.Load(url);

            // Act
            var result = _sut.Name(semanticSearchResultsWebPage.TableRows.First());

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Given_A_Html_Table_Row_Should_Extract_The_First_Card_MegasonicEye_Profile_WebPage_Url()
        {
            // Arrange
            const string expected = "https://yugioh.fandom.com/wiki/Megasonic_Eye";
            const string url = "https://yugioh.fandom.com/wiki/Special:Ask?limit=500&offset=500&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D&p=format%3Dbroadtable%2Flink%3Dall%2Fheaders%3Dshow%2Fsearchlabel%3D%E2%80%A6-20further-20results%2Fclass%3D-20sortable-20wikitable-20smwtable-20card-2Dlist&po=%3FJapanese+name%0A%3FRank%0A%3FLevel%0A%3FAttribute%0A%3FType%0A%3FMonster+type%0A%3FATK+string%3DATK%0A%3FDEF+string%3DDEF%0A&sort=&order=&eq=yes#search";
            var semanticSearchResultsWebPage = new SemanticSearchResultsWebPage(new HtmlWebPage());
            semanticSearchResultsWebPage.Load(url);

            // Act
            var result = _sut.Url(semanticSearchResultsWebPage.TableRows.First(), new Uri(url));

            // Assert
            result.Should().Be(expected);
        }
    }
}
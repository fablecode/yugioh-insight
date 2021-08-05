using FluentAssertions;
using NUnit.Framework;
using semanticsearch.infrastructure.WebPage;
using semanticsearch.tests.core;
using System;

namespace semanticsearch.infrastructure.integration.tests.WebPageTests.SemanticSearchResultsWebPageTests
{
    [TestFixture]
    [Category(TestType.Integration)]
    public class LoadTests
    {
        private SemanticSearchResultsWebPage _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SemanticSearchResultsWebPage(new HtmlWebPage());
        }

        [Test]
        public void Given_Semantic_WebPage_Url_Should_Load_WebPage_Without_Throwing_Any_Exceptions()
        {
            // Arrange
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D&po=%3FJapanese+name%0D%0A%3FRank%0D%0A%3FLevel%0D%0A%3FAttribute%0D%0A%3FType%0D%0A%3FMonster+type%0D%0A%3FATK+string%3DATK%0D%0A%3FDEF+string%3DDEF%0D%0A&eq=yes&p%5Bformat%5D=broadtable&sort_num=&order_num=ASC&p%5Blimit%5D=500&p%5Boffset%5D=&p%5Blink%5D=all&p%5Bsort%5D=&p%5Bheaders%5D=show&p%5Bmainlabel%5D=&p%5Bintro%5D=&p%5Boutro%5D=&p%5Bsearchlabel%5D=%E2%80%A6+further+results&p%5Bdefault%5D=&p%5Bclass%5D=+sortable+wikitable+smwtable+card-list&eq=yes";
            
            // Act
            Action act = () => _sut.Load(url);

            // Assert
            act.Should().NotThrow();
        }

        [Test]
        public void Given_Semantic_WebPage_Url_Should_Load_WebPage_And_All_Card_TableRows()
        {
            // Arrange
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D&po=%3FJapanese+name%0D%0A%3FRank%0D%0A%3FLevel%0D%0A%3FAttribute%0D%0A%3FType%0D%0A%3FMonster+type%0D%0A%3FATK+string%3DATK%0D%0A%3FDEF+string%3DDEF%0D%0A&eq=yes&p%5Bformat%5D=broadtable&sort_num=&order_num=ASC&p%5Blimit%5D=500&p%5Boffset%5D=&p%5Blink%5D=all&p%5Bsort%5D=&p%5Bheaders%5D=show&p%5Bmainlabel%5D=&p%5Bintro%5D=&p%5Boutro%5D=&p%5Bsearchlabel%5D=%E2%80%A6+further+results&p%5Bdefault%5D=&p%5Bclass%5D=+sortable+wikitable+smwtable+card-list&eq=yes";
            
            // Act
            _sut.Load(url);

            // Assert
            _sut.TableRows.Should().HaveCountGreaterOrEqualTo(181);
        }

        [Test]
        public void Given_Semantic_WebPage_Url_Should_Next_Page_Should_Not_Be_Null()
        {
            // Arrange
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D&po=%3FJapanese+name%0D%0A%3FRank%0D%0A%3FLevel%0D%0A%3FAttribute%0D%0A%3FType%0D%0A%3FMonster+type%0D%0A%3FATK+string%3DATK%0D%0A%3FDEF+string%3DDEF%0D%0A&eq=yes&p%5Bformat%5D=broadtable&sort_num=&order_num=ASC&p%5Blimit%5D=500&p%5Boffset%5D=&p%5Blink%5D=all&p%5Bsort%5D=&p%5Bheaders%5D=show&p%5Bmainlabel%5D=&p%5Bintro%5D=&p%5Boutro%5D=&p%5Bsearchlabel%5D=%E2%80%A6+further+results&p%5Bdefault%5D=&p%5Bclass%5D=+sortable+wikitable+smwtable+card-list&eq=yes";
            
            // Act
            _sut.Load(url);

            // Assert
            _sut.NextPage.Should().NotBeNull();
        }

        [Test]
        public void Given_Semantic_WebPage_Url_Should_Next_Page_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D&po=%3FJapanese+name%0D%0A%3FRank%0D%0A%3FLevel%0D%0A%3FAttribute%0D%0A%3FType%0D%0A%3FMonster+type%0D%0A%3FATK+string%3DATK%0D%0A%3FDEF+string%3DDEF%0D%0A&eq=yes&p%5Bformat%5D=broadtable&sort_num=&order_num=ASC&p%5Blimit%5D=500&p%5Boffset%5D=&p%5Blink%5D=all&p%5Bsort%5D=&p%5Bheaders%5D=show&p%5Bmainlabel%5D=&p%5Bintro%5D=&p%5Boutro%5D=&p%5Bsearchlabel%5D=%E2%80%A6+further+results&p%5Bdefault%5D=&p%5Bclass%5D=+sortable+wikitable+smwtable+card-list&eq=yes";
            
            // Act
            _sut.Load(url);

            // Assert
            _sut.NextPageLink().Should().NotBeNullOrEmpty();
        }

        [Test]
        public void Given_Semantic_WebPage_Url_HasNextPage_Should_Be_True()
        {
            // Arrange
            const string url = "http://yugioh.fandom.com/index.php?title=Special%3AAsk&q=%5B%5BClass+1%3A%3AOfficial%5D%5D+%5B%5BCard+type%3A%3ANormal+Monster%5D%5D&po=%3FJapanese+name%0D%0A%3FRank%0D%0A%3FLevel%0D%0A%3FAttribute%0D%0A%3FType%0D%0A%3FMonster+type%0D%0A%3FATK+string%3DATK%0D%0A%3FDEF+string%3DDEF%0D%0A&eq=yes&p%5Bformat%5D=broadtable&sort_num=&order_num=ASC&p%5Blimit%5D=500&p%5Boffset%5D=&p%5Blink%5D=all&p%5Bsort%5D=&p%5Bheaders%5D=show&p%5Bmainlabel%5D=&p%5Bintro%5D=&p%5Boutro%5D=&p%5Bsearchlabel%5D=%E2%80%A6+further+results&p%5Bdefault%5D=&p%5Bclass%5D=+sortable+wikitable+smwtable+card-list&eq=yes";
            
            // Act
            _sut.Load(url);

            // Assert
            _sut.HasNextPage.Should().BeTrue();
        }
    }
}
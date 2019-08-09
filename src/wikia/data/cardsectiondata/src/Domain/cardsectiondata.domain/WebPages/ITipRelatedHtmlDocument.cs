using HtmlAgilityPack;

namespace cardsectiondata.domain.WebPages
{
    public interface ITipRelatedHtmlDocument
    {
        HtmlNode GetTable(HtmlDocument document);
        string GetUrl(HtmlDocument document);
    }
}
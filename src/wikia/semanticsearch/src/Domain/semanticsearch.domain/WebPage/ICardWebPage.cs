using System;
using semanticsearch.core.Model;

namespace semanticsearch.domain.WebPage
{
    public interface ICardWebPage
    {
        YugiohCard GetYugiohCard(string url);

        YugiohCard GetYugiohCard(Uri url);
    }
}
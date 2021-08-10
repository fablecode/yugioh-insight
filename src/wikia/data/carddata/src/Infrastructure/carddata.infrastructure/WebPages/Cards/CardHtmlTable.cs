using System.Collections.Generic;
using System.Linq;
using carddata.domain.WebPages.Cards;
using HtmlAgilityPack;

namespace carddata.infrastructure.WebPages.Cards
{
    public class CardHtmlTable : ICardHtmlTable
    {
        public const string Name = "English";
        public const string Number = "Passcode";
        public const string CardType = "Card type";
        public const string Property = "Property";
        public const string Attribute = "Attribute";
        public const string Level = "Level";
        public const string Rank = "Rank";
        public const string LinkArrows = "Link Arrows";
        public const string Materials = "Materials";
        public const string CardEffectTypes = "Card effect types";
        public const string PendulumScale = "Pendulum Scale";
        public static readonly string[] AtkAndDef = { "ATK / DEF", "ATK/DEF" };
        public static readonly string[] AtkAndLink = { "ATK / LINK", "ATK/LINK" };
        public static readonly string[] Types = { "Type", "Types" };

        public string GetValue(string[] keys, HtmlNode htmlTable)
        {
            return keys
                .Select(key => GetValue(key, htmlTable))
                .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
        }

        public string GetValue(string key, HtmlNode htmlTable)
        {
            var cardTable = ProfileData(htmlTable);

            var value = string.Empty;

            if (cardTable == null || string.IsNullOrWhiteSpace(key))
                return value;

            var isKeyFound = cardTable.TryGetValue(key, out value);

            if (isKeyFound)
            {
                value = value.Trim().Replace("\n", null);
            }

            return value;
        }

        public Dictionary<string, string> ProfileData(HtmlNode htmlTable)
        {
            var cardTable = new Dictionary<string, string>();

            var htmlTableRows = htmlTable?.SelectNodes("./tbody/tr");

            if (htmlTableRows != null && htmlTableRows.Any())
            {
                foreach (var row in htmlTableRows)
                {
                    var key = row.SelectSingleNode("./th[contains(@class, 'cardtablerowheader')]");
                    var value = row.SelectSingleNode("./td[contains(@class, 'cardtablerowdata')]");

                    if (key != null && value != null && !cardTable.ContainsKey(key.InnerText))
                    {
                        var cardEffectTypes = key.InnerText == "Card effect types" ? string.Join(",", value.SelectNodes("./ul/li").Select(t => t.InnerText.Trim())) : value.InnerText.Trim();

                        cardTable.Add(key.InnerText.Trim(), cardEffectTypes);
                    }
                }
            }

            return cardTable;
        }
    }
}
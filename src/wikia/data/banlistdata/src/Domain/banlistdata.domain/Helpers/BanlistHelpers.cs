using System;
using System.Text.RegularExpressions;
using banlistdata.core.Enums;
using banlistdata.core.Models;

namespace banlistdata.domain.Helpers
{
    public static class BanlistHelpers
    {
        public static BanlistArticleSummary ExtractBanlistArticleDetails(int articleId, string titleText)
        {
            var regex = new Regex(@"((?:TCG|OCG?)) in effect since ((?:Jan(?:uary)?|Feb(?:ruary)?|Mar(?:ch)?|Apr(?:il)?|May|Jun(?:e)?|Jul(?:y)?|Aug(?:ust)?|Sep(?:tember)?|Sept|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?)) ((?:\d{1,2}?)), ((?:\d{4}?))");

            var match = regex.Match(titleText);

            if (match.Success)
            {
                var format = (BanlistType)Enum.Parse(typeof(BanlistType), match.Groups[1].Value, true);
                var startDate = DateTime.Parse($"{match.Groups[2]} {match.Groups[3]}, {match.Groups[4]}");

                var response = new BanlistArticleSummary
                {
                    BanlistType = format,
                    StartDate = startDate,
                    ArticleId = articleId
                };


                return response;
            }

            return null;
        }
    }
}
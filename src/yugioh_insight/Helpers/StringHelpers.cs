using System.Text.RegularExpressions;

namespace yugioh_insight.Helpers
{
    public static class StringHelpers
    {
        public static string RemoveBetween(string text, char begin, char end)
        {
            var regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(text, string.Empty).Trim();
        }
    }
}
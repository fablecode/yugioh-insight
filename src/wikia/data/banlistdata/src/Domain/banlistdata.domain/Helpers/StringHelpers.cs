using System.Text.RegularExpressions;

namespace banlistdata.domain.Helpers
{
    public static class StringHelpers
    {
        public static string ArchetypeNameFromListTitle(string text)
        {
            var regex = new Regex("\"([^\"]*?)\"");
            var match = regex.Match(text);

            return match.Success ? match.Value.Trim('"') : null;
        }

        public static string RemoveBetween(string text, char begin, char end)
        {
            var regex = new Regex($"\\{begin}.*?\\{end}");
            return regex.Replace(text, string.Empty).Trim();
        }
    }
}
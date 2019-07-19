using System.Text.RegularExpressions;

namespace archetypedata.domain.Helpers
{
    public static class StringHelpers
    {
        public static string ArchetypeNameFromListTitle(string text)
        {
            var regex = new Regex("\"([^\"]*?)\"");
            var match = regex.Match(text);

            return match.Success ? match.Value.Trim('"') : null;
        }
    }
}
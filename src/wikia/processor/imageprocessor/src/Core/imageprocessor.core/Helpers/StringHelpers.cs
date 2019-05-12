using System.IO;

namespace imageprocessor.core.Helpers
{
    public static class StringHelpers
    {
        public static string SanitizeFileName(this string name)
        {
            return string.Concat(name.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
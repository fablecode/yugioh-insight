using System.Text.RegularExpressions;

namespace carddata.core.Helpers
{
    public static class ImageHelper
    {
        public static string ExtractImageUrl(string imageUrl)
        {
            return string.IsNullOrWhiteSpace(imageUrl) ? null : new Regex(@"(?<Protocol>\w+):(.+?).(jpg|jpeg|tif|tiff|png|gif|bmp|wmf)").Match(imageUrl).Value;
        }
    }
}
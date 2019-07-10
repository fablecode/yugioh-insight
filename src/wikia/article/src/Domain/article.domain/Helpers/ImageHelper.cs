using System.Text.RegularExpressions;

namespace article.domain.Helpers
{
    public static class ImageHelper
    {
        public static string ExtractImageUrl(string imageUrl)
        {
            return string.IsNullOrWhiteSpace(imageUrl) ? null : new Regex(@"(?<Protocol>\w+):(.+?).(jpg|jpeg|tif|tiff|png|gif|bmp|wmf)").Match(imageUrl).Value;
        }
    }
}
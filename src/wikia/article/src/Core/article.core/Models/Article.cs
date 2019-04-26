using System;
using wikia.Models.Article.AlphabeticalList;

namespace article.core.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
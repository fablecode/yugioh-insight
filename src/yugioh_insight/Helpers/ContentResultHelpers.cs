using System.Collections.Generic;
using wikia.Models.Article.Simple;

namespace yugioh_insight.Helpers
{
    public static class ContentResultHelpers
    {
        public static List<string> GetSectionContentList(Section s)
        {
            var content = new List<string>();

            foreach (var c in s.Content)
            {
                GetContentList(c.Elements, content);
            }

            return content;
        }

        public static void GetContentList(ListElement[] elementList, List<string> contentlist)
        {
            if (elementList != null)
            {
                foreach (var e in elementList)
                {
                    if (e != null)
                    {
                        if (!string.IsNullOrEmpty(e.Text))
                            contentlist.Add(e.Text);

                        GetContentList(e.Elements, contentlist);
                    }
                }
            }
        }
    }
}
using System.Collections.Generic;
using wikia.Models.Article.Simple;

namespace cardsectiondata.domain.Helpers
{
    public static class SectionHelper
    {
        public static List<string> GetSectionContentList(Section cardTipSection)
        {
            var content = new List<string>();

            foreach (var c in cardTipSection.Content)
            {
                GetContentList(c.Elements, content);
            }

            return content;
        }

        public static void GetContentList(IEnumerable<ListElement> elementList, List<string> contentlist)
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
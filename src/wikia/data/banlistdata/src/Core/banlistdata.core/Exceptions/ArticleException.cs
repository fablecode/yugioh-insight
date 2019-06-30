using System;

namespace banlistdata.core.Exceptions
{
    public class ArticleException
    {
        public string Article { get; set; }

        public Exception Exception { get; set; }
    }
}
using System;

namespace carddata.core.Exceptions
{
    public class ArticleException
    {
        public string Article { get; set; }

        public Exception Exception { get; set; }
    }
}
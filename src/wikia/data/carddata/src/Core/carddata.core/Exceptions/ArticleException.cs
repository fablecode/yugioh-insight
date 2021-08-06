using System;
using carddata.core.Models;

namespace carddata.core.Exceptions
{
    public class ArticleException
    {
        public Article Article { get; set; }

        public Exception Exception { get; set; }
    }
}
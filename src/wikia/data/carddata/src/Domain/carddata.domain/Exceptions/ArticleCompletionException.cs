using System;

namespace carddata.domain.Exceptions
{
    public class ArticleCompletionException : Exception
    {
        public ArticleCompletionException(string exceptionMessage)
            : base (exceptionMessage)
        {
        }
    }
}
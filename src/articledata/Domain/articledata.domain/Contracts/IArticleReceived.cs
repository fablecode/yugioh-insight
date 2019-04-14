using System;

namespace articledata.domain.Contracts
{
    public interface IArticleReceived
    {
        int Id { get; set; }
        DateTimeOffset ArticleDateTime { get; }
    }
}
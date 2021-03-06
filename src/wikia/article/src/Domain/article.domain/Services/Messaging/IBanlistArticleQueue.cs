﻿using System.Threading.Tasks;
using wikia.Models.Article.AlphabeticalList;

namespace article.domain.Services.Messaging
{
    public interface IBanlistArticleQueue
    {
        Task Publish(UnexpandedArticle article);
    }
}
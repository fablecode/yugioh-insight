using System;
using System.Threading.Tasks;
using cardsectiondata.core.Models;
using cardsectiondata.core.Processor;

namespace cardsectiondata.domain.ArticleList.Item
{
    public class CardTipItemProcessor : IArticleItemProcessor
    {
        public Task<ArticleTaskResult> ProcessItem(Article item)
        {
            throw new NotImplementedException();
        }

        public bool Handles(string category)
        {
            throw new NotImplementedException();
        }
    }
}
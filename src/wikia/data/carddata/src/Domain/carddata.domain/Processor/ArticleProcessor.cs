using System;
using System.Threading.Tasks;
using carddata.core.Exceptions;
using carddata.core.Models;
using carddata.core.Processor;
using Newtonsoft.Json;

namespace carddata.domain.Processor
{
    public class ArticleProcessor : IArticleProcessor
    {
        private readonly IArticleDataFlow _articleDataFlow;

        public ArticleProcessor(IArticleDataFlow articleDataFlow)
        {
            _articleDataFlow = articleDataFlow;
        }

        public async Task<ArticleConsumerResult> Process(Article article)
        {
            var articleJson = JsonConvert.SerializeObject(article);

            var response = new ArticleConsumerResult();
            response.Article = articleJson;

            try
            {
                var result = await _articleDataFlow.ProcessDataFlow(article);

                if (result.IsSuccessful)
                {
                    response.IsSuccessfullyProcessed = true;
                }
            }
            catch (Exception ex )
            {
               response.Failed = new ArticleException { Article = articleJson, Exception = ex };
            }

            return response;
        }
    }
}
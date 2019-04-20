using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using carddata.core.Models;
using carddata.core.Processor;
using carddata.domain.Services.Messaging;
using carddata.domain.WebPages.Cards;

namespace carddata.domain.Processor
{
    public class ArticleProcessor : IArticleProcessor
    {
        public Task<ArticleConsumerResult> Process(Article article)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ArticleDataFlow
    {
        private readonly ICardWebPage _cardWebPage;
        private readonly IYugiohCardQueue _yugiohCardQueue;
        private BufferBlock<Article> _articleBufferBlock;
        private TransformBlock<Article, YugiohCard> _yugiohDataTransformBlock;
        private ActionBlock<YugiohCard> _publishToQueueActionBlock;

        public ArticleDataFlow(ICardWebPage cardWebPage, IYugiohCardQueue yugiohCardQueue)
        {
            _cardWebPage = cardWebPage;
            _yugiohCardQueue = yugiohCardQueue;
        }

        public ArticleDataFlow()
        {
            // Pipeline members
            var processorCount = Environment.ProcessorCount;

            _articleBufferBlock = new BufferBlock<Article>();
            _yugiohDataTransformBlock = new TransformBlock<Article, YugiohCard>(article => _cardWebPage.GetYugiohCard(new Uri(article.Url)));
            _publishToQueueActionBlock = new ActionBlock<YugiohCard>(async yugiohCard =>
            {
                await _yugiohCardQueue.Publish(yugiohCard);
            },
            // Specify a maximum degree of parallelism.
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = processorCount
            });

            // Form the pipeline
            _articleBufferBlock.LinkTo(_yugiohDataTransformBlock);

        }
    }
}
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using semanticsearch.core.Model;
using semanticsearch.core.Search;

namespace semanticsearch.domain.Search
{
    public class SemanticSearchProcessor : ISemanticSearchProcessor
    {
        private readonly ISemanticSearchProducer _semanticSearchProducer;
        private readonly ISemanticSearchConsumer _semanticSearchConsumer;
        public BufferBlock<SemanticCard> CardBufferBlock { get; private set; }
        public ActionBlock<SemanticCardPublishResult> CardActionBlock { get; private set; }

        public SemanticSearchProcessor(ISemanticSearchProducer semanticSearchProducer, ISemanticSearchConsumer semanticSearchConsumer)
        {
            _semanticSearchProducer = semanticSearchProducer;
            _semanticSearchConsumer = semanticSearchConsumer;
        }

        public async Task<SemanticSearchCardTaskResult> ProcessUrl(string url)
        {
            var response = new SemanticSearchCardTaskResult { Url = url };

            // Data flow options
            var processorCount = Environment.ProcessorCount;
            var nonGreedy = new ExecutionDataflowBlockOptions { BoundedCapacity = processorCount, MaxDegreeOfParallelism = processorCount };
            var flowComplete = new DataflowLinkOptions { PropagateCompletion = true };

            // Pipeline members
            CardBufferBlock = new BufferBlock<SemanticCard>();
            var cardPublishTransformBlock = new TransformBlock<SemanticCard, SemanticCardPublishResult>(semanticCard => _semanticSearchConsumer.Process(semanticCard), nonGreedy);
            CardActionBlock = new ActionBlock<SemanticCardPublishResult>(delegate (SemanticCardPublishResult result)
            {
                if(result.IsSuccessful)
                    response.Processed += 1;
                else
                {
                    response.Failed.Add(result.Exception);
                }
            });

            // Form the pipeline
            CardBufferBlock.LinkTo(cardPublishTransformBlock, flowComplete);
            cardPublishTransformBlock.LinkTo(CardActionBlock, flowComplete);

            // Process "Category" and generate article batch data
            await _semanticSearchProducer.Producer(url, CardBufferBlock);

            // Mark the head of the pipeline as complete. The continuation tasks  
            // propagate completion through the pipeline as each part of the  
            // pipeline finishes.
            await CardActionBlock.Completion;

            response.IsSuccessful = true;

            return response;
        }
    }
}
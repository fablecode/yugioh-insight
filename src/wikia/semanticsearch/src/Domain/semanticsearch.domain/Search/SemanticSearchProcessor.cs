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
        private readonly ISemanticSearchPublish _semanticSearchPublish;

        public SemanticSearchProcessor(ISemanticSearchProducer semanticSearchProducer, ISemanticSearchPublish semanticSearchPublish)
        {
            _semanticSearchProducer = semanticSearchProducer;
            _semanticSearchPublish = semanticSearchPublish;
        }

        public async Task<SemanticSearchBatchTaskResult> ProcessUrl(string category, string url)
        {
            var response = new SemanticSearchBatchTaskResult { Url = url };

            // Data flow options
            var processorCount = Environment.ProcessorCount;
            var nonGreedy = new ExecutionDataflowBlockOptions { BoundedCapacity = processorCount, MaxDegreeOfParallelism = processorCount };
            var flowComplete = new DataflowLinkOptions { PropagateCompletion = true };

            // Pipeline members
            var cardBufferBlock = new BufferBlock<SemanticCard>();
            var cardPublishTransformBlock = new TransformBlock<SemanticCard, SemanticSearchBatchTaskResult>(semanticCards => _semanticSearchPublish.Publish(category, semanticCards), nonGreedy);
            var cardActionBlock = new ActionBlock<SemanticSearchBatchTaskResult>(delegate (SemanticSearchBatchTaskResult result)
            {
                response.Processed += result.Processed;
                response.Failed.AddRange(result.Failed);
            });

            // Form the pipeline
            cardBufferBlock.LinkTo(cardPublishTransformBlock, flowComplete);
            cardPublishTransformBlock.LinkTo(cardActionBlock, flowComplete);

            // Publish "Category" and generate article batch data
            await _semanticSearchProducer.Producer(url, cardBufferBlock);

            // Mark the head of the pipeline as complete. The continuation tasks  
            // propagate completion through the pipeline as each part of the  
            // pipeline finishes.
            await cardActionBlock.Completion;

            return response;
        }
    }
}
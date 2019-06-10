using MediatR;
using Microsoft.Extensions.Options;
using semanticsearch.application.Configuration;
using semanticsearch.core.Search;
using System.Threading;
using System.Threading.Tasks;

namespace semanticsearch.application.ScheduledTasks.CardSearch
{
    public class SemanticSearchCardTaskHandler : IRequestHandler<SemanticSearchCardTask, SemanticSearchCardTaskResult>
    {
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly ISemanticSearchProcessor _semanticSearchProcessor;

        public SemanticSearchCardTaskHandler
        (
            IOptions<AppSettings> appSettingsOptions, 
            ISemanticSearchProcessor semanticSearchProcessor
        )
        {
            _appSettingsOptions = appSettingsOptions;
            _semanticSearchProcessor = semanticSearchProcessor;
        }
        public async Task<SemanticSearchCardTaskResult> Handle(SemanticSearchCardTask request, CancellationToken cancellationToken)
        {
            var semanticSearchCardTaskResult = new SemanticSearchCardTaskResult();

            foreach (var (_, url) in _appSettingsOptions.Value.CardSearchUrls)
            {
                await _semanticSearchProcessor.ProcessUrl(url);
            }

            semanticSearchCardTaskResult.IsSuccessful = true;

            return semanticSearchCardTaskResult;
        }
    }
}
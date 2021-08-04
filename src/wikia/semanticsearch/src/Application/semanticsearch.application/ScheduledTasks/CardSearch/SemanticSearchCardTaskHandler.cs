using MediatR;
using Microsoft.Extensions.Options;
using semanticsearch.application.Configuration;
using semanticsearch.core.Search;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace semanticsearch.application.ScheduledTasks.CardSearch
{
    public class SemanticSearchCardTaskHandler : IRequestHandler<SemanticSearchCardTask, SemanticSearchCardTaskResult>
    {
        private readonly ILogger<SemanticSearchCardTaskHandler> _logger;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly ISemanticSearchProcessor _semanticSearchProcessor;

        public SemanticSearchCardTaskHandler
        (
            ILogger<SemanticSearchCardTaskHandler> logger,
            IOptions<AppSettings> appSettingsOptions, 
            ISemanticSearchProcessor semanticSearchProcessor
        )
        {
            _logger = logger;
            _appSettingsOptions = appSettingsOptions;
            _semanticSearchProcessor = semanticSearchProcessor;
        }
        public async Task<SemanticSearchCardTaskResult> Handle(SemanticSearchCardTask request, CancellationToken cancellationToken)
        {
            var semanticSearchCardTaskResult = new SemanticSearchCardTaskResult();

            foreach (var (category, url) in _appSettingsOptions.Value.CardSearchUrls)
            {
                _logger.LogInformation("Processing semantic category '{Category}'.", category);
                await _semanticSearchProcessor.ProcessUrl(url);
                _logger.LogInformation("Finished processing semantic category '{Category}'.", category);
            }

            semanticSearchCardTaskResult.IsSuccessful = true;

            return semanticSearchCardTaskResult;
        }
    }
}
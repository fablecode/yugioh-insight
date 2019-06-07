using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using semanticsearch.application.Configuration;
using semanticsearch.core.Search;

namespace semanticsearch.application.ScheduledTasks.CardSearch
{
    public class SemanticSearchCardTaskHandler : IRequestHandler<SemanticSearchCardTask, SemanticSearchCardTaskResult>
    {
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly ISemanticSearchProcessor _semanticSearchProcessor;
        private readonly ILogger<SemanticSearchCardTaskHandler> _logger;

        public SemanticSearchCardTaskHandler
        (
            IOptions<AppSettings> appSettingsOptions, 
            ISemanticSearchProcessor semanticSearchProcessor, 
            ILogger<SemanticSearchCardTaskHandler> logger
        )
        {
            _appSettingsOptions = appSettingsOptions;
            _semanticSearchProcessor = semanticSearchProcessor;
            _logger = logger;
        }
        public async Task<SemanticSearchCardTaskResult> Handle(SemanticSearchCardTask request, CancellationToken cancellationToken)
        {
            var semanticSearchCardTaskResult = new SemanticSearchCardTaskResult();

            foreach (var (category, url) in _appSettingsOptions.Value.CardSearchUrls)
            {
                try
                {
                    await _semanticSearchProcessor.ProcessUrl(url);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to process semantic url: @{SemanticUrl}. Exception: @{Exception}", url, ex);
                }
            }

            return semanticSearchCardTaskResult;
        }
    }
}
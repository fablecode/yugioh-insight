using System;
using System.Threading;
using System.Threading.Tasks;
using banlistprocessor.core.Models;
using banlistprocessor.core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace banlistprocessor.application.MessageConsumers.BanlistData
{
    public class BanlistDataConsumerHandler : IRequestHandler<BanlistDataConsumer, BanlistDataConsumerResult>
    {
        private readonly IBanlistService _banlistService;
        private readonly ILogger<BanlistDataConsumerHandler> _logger;

        public BanlistDataConsumerHandler(IBanlistService banlistService, ILogger<BanlistDataConsumerHandler> logger)
        {
            _banlistService = banlistService;
            _logger = logger;
        }

        public async Task<BanlistDataConsumerResult> Handle(BanlistDataConsumer request, CancellationToken cancellationToken)
        {
            var banlistDataConsumerResult = new BanlistDataConsumerResult();

            try
            {
                var yugiohBanlist = JsonConvert.DeserializeObject<YugiohBanlist>(request.Message);

                banlistDataConsumerResult.Banlist = yugiohBanlist;

                var banlistExists = await _banlistService.BanlistExist(yugiohBanlist.ArticleId);

                var result = banlistExists
                    ? await _banlistService.Update(yugiohBanlist)
                    : await _banlistService.Add(yugiohBanlist);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Argument {@Param} was null. Message: {@Message}: ", ex.ParamName, request.Message);
            }

            return banlistDataConsumerResult;
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Enums;
using article.domain.Banlist.Processor;
using FluentValidation;
using MediatR;

namespace article.application.ScheduledTasks.LatestBanlist
{
    public class BanlistInformationTaskHandler : IRequestHandler<BanlistInformationTask, BanlistInformationTaskResult>
    {
        private readonly IArticleCategoryProcessor _articleCategoryProcessor;
        private readonly IValidator<BanlistInformationTask> _validator;
        private readonly IBanlistProcessor _banlistProcessor;

        public BanlistInformationTaskHandler
        (
            IArticleCategoryProcessor articleCategoryProcessor, 
            IValidator<BanlistInformationTask> validator,
            IBanlistProcessor banlistProcessor
        )
        {
            _articleCategoryProcessor = articleCategoryProcessor;
            _validator = validator;
            _banlistProcessor = banlistProcessor;
        }

        public async Task<BanlistInformationTaskResult> Handle(BanlistInformationTask request, CancellationToken cancellationToken)
        {
            var response = new BanlistInformationTaskResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var categoryResult = await _articleCategoryProcessor.Process(request.Category, request.PageSize);

                await _banlistProcessor.Process(BanlistType.Tcg);
                await _banlistProcessor.Process(BanlistType.Ocg);

                response.ArticleTaskResults = categoryResult;
            }
            else
            {
                response.Errors = validationResults.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return response;
        }
    }
}
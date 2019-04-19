using articledata.core.ArticleList.Processor;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace articledata.application.ScheduledTasks.CardInformation
{
    public class CardInformationTaskHandler : IRequestHandler<CardInformationTask, CardInformationTaskResult>
    {
        private readonly IArticleCategoryProcessor _articleCategoryProcessor;
        private readonly IValidator<CardInformationTask> _validator;

        public CardInformationTaskHandler(IArticleCategoryProcessor articleCategoryProcessor, IValidator<CardInformationTask> validator)
        {
            _articleCategoryProcessor = articleCategoryProcessor;
            _validator = validator;
        }
        public async Task<CardInformationTaskResult> Handle(CardInformationTask request, CancellationToken cancellationToken)
        {
            var cardInformationTaskResult = new CardInformationTaskResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                //var results = await _articleCategoryProcessor.Process(request.Categories, request.PageSize);

                //cardInformationTaskResult.ArticleTaskResults = results;
            }

            return cardInformationTaskResult;
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using FluentValidation;
using MediatR;

namespace article.application.ScheduledTasks.Articles
{
    public class ArticleInformationTaskHandler : IRequestHandler<ArticleInformationTask, ArticleInformationTaskResult>
    {
        private readonly IArticleCategoryProcessor _articleCategoryProcessor;
        private readonly IValidator<ArticleInformationTask> _validator;

        public ArticleInformationTaskHandler
        (
            IArticleCategoryProcessor articleCategoryProcessor,
            IValidator<ArticleInformationTask> validator
        )
        {
            _articleCategoryProcessor = articleCategoryProcessor;
            _validator = validator;
        }

        public async Task<ArticleInformationTaskResult> Handle(ArticleInformationTask request, CancellationToken cancellationToken)
        {
            var response = new ArticleInformationTaskResult();

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var categoryResult = await _articleCategoryProcessor.Process(request.Category, request.PageSize);

                response.ArticleTaskResults = categoryResult;
                response.IsSuccessful = true;
            }
            else
            {
                response.Errors = validationResults.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return response;
        }
    }
}
using article.core.ArticleList.Processor;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace article.application.ScheduledTasks.Archetype
{
    public class ArchetypeInformationTaskHandler : IRequestHandler<ArchetypeInformationTask, ArchetypeInformationTaskResult>
    {
        private readonly IArticleCategoryProcessor _articleCategoryProcessor;
        private readonly IValidator<ArchetypeInformationTask> _validator;

        public ArchetypeInformationTaskHandler(IArticleCategoryProcessor articleCategoryProcessor, IValidator<ArchetypeInformationTask> validator)
        {
            _articleCategoryProcessor = articleCategoryProcessor;
            _validator = validator;
        }

        public async Task<ArchetypeInformationTaskResult> Handle(ArchetypeInformationTask request, CancellationToken cancellationToken)
        {
            var archetypeInformationTaskResult = new ArchetypeInformationTaskResult();

            var validationResults = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResults.IsValid)
            {
                var results = await _articleCategoryProcessor.Process(request.Category, request.PageSize);

                archetypeInformationTaskResult.ArticleTaskResult = results;
            }
            else
            {
                archetypeInformationTaskResult.Errors = validationResults.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return archetypeInformationTaskResult;
        }
    }
}
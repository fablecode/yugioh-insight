using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using article.core.ArticleList.Processor;
using article.core.Models;
using FluentValidation;
using MediatR;

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

            var validationResults = _validator.Validate(request);

            if (validationResults.IsValid)
            {
                var results = await _articleCategoryProcessor.Process(request.Categories, request.PageSize);

                archetypeInformationTaskResult.ArticleTaskResults = results;
            }
            else
            {
                archetypeInformationTaskResult.Errors = validationResults.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return archetypeInformationTaskResult;
        }
    }
}
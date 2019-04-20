using articledata.application.ScheduledTasks.CardInformation;
using articledata.core.ArticleList.Processor;
using articledata.domain.ArticleList.DataSource;
using articledata.domain.ArticleList.Item;
using articledata.domain.ArticleList.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using wikia.Api;

namespace articledata.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddCqrs()
                .AddValidation()
                .AddDomainServices();

            return services;
        }

        private static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationInstaller).GetTypeInfo().Assembly);

            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IArticleCategoryDataSource, ArticleCategoryDataSource>();
            services.AddTransient<IArticleBatchProcessor, ArticleBatchProcessor>();
            services.AddTransient<IArticleCategoryProcessor, ArticleCategoryProcessor>();
            services.AddTransient<IArticleProcessor, ArticleProcessor>();

            services.AddTransient<IArticleItemProcessor, CardItemProcessor>();


            var appSettings = services.BuildServiceProvider().GetService<IOptions<Configuration.AppSettings>>();

            services.AddSingleton<IWikiArticleList>(new WikiArticleList(appSettings.Value.WikiaDomainUrl));

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CardInformationTask>, CardInformationTaskValidator>();

            return services;
        }
    }
}
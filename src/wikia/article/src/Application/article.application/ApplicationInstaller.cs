using article.application.ScheduledTasks.CardInformation;
using article.core.ArticleList.Processor;
using article.domain.ArticleList.DataSource;
using article.domain.ArticleList.Item;
using article.domain.ArticleList.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using article.application.Decorators.Loggers;
using article.core.ArticleList.DataSource;
using wikia.Api;

namespace article.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddCqrs()
                .AddValidation()
                .AddDomainServices()
                .AddDecorators();

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

        private static IServiceCollection AddDecorators(this IServiceCollection services)
        {
            services.Decorate<IArticleProcessor, ArticleProcessorLoggerDecorator>();

            return services;
        }
    }
}
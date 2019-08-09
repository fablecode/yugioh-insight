using cardsectiondata.application.Configuration;
using cardsectiondata.core.Processor;
using cardsectiondata.domain.ArticleList.Item;
using cardsectiondata.domain.ArticleList.Processor;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Reflection;
using wikia.Api;

namespace cardsectiondata.application
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
            services.AddTransient<IArticleProcessor, ArticleProcessor>();
            services.AddTransient<ICardSectionProcessor, CardSectionProcessor>();

            services.AddTransient<IArticleItemProcessor, CardTipItemProcessor>();
            services.AddTransient<IArticleItemProcessor, CardRulingItemProcessor>();
            services.AddTransient<IArticleItemProcessor, CardTriviaItemProcessor>();

            var buildServiceProvider = services.BuildServiceProvider();

            var appSettings = buildServiceProvider.GetService<IOptions<AppSettings>>();

            services.AddSingleton<IWikiArticle>(new WikiArticle(appSettings.Value.WikiaDomainUrl, buildServiceProvider.GetService<IHttpClientFactory>()));

            return services;
        }


        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            return services;
        }

    }
}
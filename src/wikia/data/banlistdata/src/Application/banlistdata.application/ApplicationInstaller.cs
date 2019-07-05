using System.Net.Http;
using System.Reflection;
using banlistdata.application.MessageConsumers.BanlistInformation;
using banlistdata.core.Processor;
using banlistdata.domain.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using wikia;
using wikia.Api;

namespace banlistdata.application
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
            services.AddTransient<IBanlistProcessor, BanlistProcessor>();

            services.AddSingleton<IArticleDataFlow, ArticleDataFlow>();

            var buildServiceProvider = services.BuildServiceProvider();

            var appSettings = buildServiceProvider.GetService<IOptions<Configuration.AppSettings>>();

            services.AddSingleton<IWikiArticle>(new WikiArticle(appSettings.Value.WikiaDomainUrl, buildServiceProvider.GetService<IHttpClientFactory>()));

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<BanlistInformationConsumer>, BanlistInformationConsumerValidator>();

            return services;
        }
    }
}
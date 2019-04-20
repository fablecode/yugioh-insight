using System.Reflection;
using carddata.application.Configuration;
using carddata.application.MessageConsumers.CardInformation;
using carddata.core.Processor;
using carddata.domain.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using wikia.Api;

namespace carddata.application
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
            services.AddSingleton<IArticleProcessor, ArticleProcessor>();

            var appSettings = services.BuildServiceProvider().GetService<IOptions<AppSettings>>();

            services.AddSingleton<IWikiArticleList>(new WikiArticleList(appSettings.Value.WikiaDomainUrl));

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CardInformationConsumer>, CardInformationConsumerValidator>();

            return services;
        }
    }
}
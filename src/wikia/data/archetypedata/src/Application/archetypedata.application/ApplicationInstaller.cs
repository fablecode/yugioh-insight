using System.Net.Http;
using System.Reflection;
using archetypedata.application.MessageConsumers.ArchetypeInformation;
using archetypedata.core.Processor;
using archetypedata.domain.Processor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using wikia.Api;

namespace archetypedata.application
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
            services.AddTransient<IArchetypeCardProcessor, ArchetypeCardProcessor>();
            services.AddSingleton<IArchetypeProcessor, ArchetypeProcessor>();

            var buildServiceProvider = services.BuildServiceProvider();

            var appSettings = buildServiceProvider.GetService<IOptions<Configuration.AppSettings>>();

            services.AddSingleton<IWikiArticle>(new WikiArticle(appSettings.Value.WikiaDomainUrl, buildServiceProvider.GetService<IHttpClientFactory>()));


            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<ArchetypeInformationConsumer>, ArchetypeInformationConsumerValidator>();

            return services;
        }
    }
}
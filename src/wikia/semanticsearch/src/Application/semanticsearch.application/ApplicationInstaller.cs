using MediatR;
using Microsoft.Extensions.DependencyInjection;
using semanticsearch.core.Search;
using semanticsearch.domain.Search.Consumer;
using semanticsearch.domain.Search.Producer;
using System.Reflection;
using semanticsearch.domain.Search;

namespace semanticsearch.application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddCqrs()
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
            services.AddTransient<ISemanticSearchProducer, SemanticSearchProducer>();
            services.AddTransient<ISemanticSearchConsumer, SemanticSearchConsumer>();
            services.AddTransient<ISemanticSearchProcessor, SemanticSearchProcessor>();

            return services;
        }

    }
}
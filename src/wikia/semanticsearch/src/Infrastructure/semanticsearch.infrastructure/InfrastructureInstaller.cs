using System;
using Microsoft.Extensions.DependencyInjection;
using semanticsearch.domain.Messaging.Exchanges;
using semanticsearch.domain.WebPage;
using semanticsearch.infrastructure.Messaging.Exchanges;
using semanticsearch.infrastructure.WebPage;

namespace semanticsearch.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services
                .AddHtmlAgility()
                .AddMessagingServices();

            return services;
        }

        public static IServiceCollection AddHtmlAgility(this IServiceCollection services)
        {
            services.AddTransient<IHtmlWebPage, HtmlWebPage>();

            return services;
        }
        public static IServiceCollection AddMessagingServices(this IServiceCollection services)
        {
            services.AddTransient<IArticleHeaderExchange, ArticleHeaderExchange>();
            services.AddTransient<ISemanticSearchResultsWebPage, SemanticSearchResultsWebPage>();
            services.AddTransient<ISemanticCardSearchResultsWebPage, SemanticCardSearchResultsWebPage>();

            return services;
        }

    }
}

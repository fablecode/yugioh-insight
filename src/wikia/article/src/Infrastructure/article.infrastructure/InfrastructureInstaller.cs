using article.domain;
using article.domain.Services.Messaging.Cards;
using article.domain.WebPages;
using article.domain.WebPages.Banlists;
using article.infrastructure.Services.Messaging.Cards;
using article.infrastructure.WebPages.Banlists;
using Microsoft.Extensions.DependencyInjection;

namespace article.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddMessagingServices();

            return services;
        }
        public static IServiceCollection AddMessagingServices(this IServiceCollection services)
        {
            services.AddTransient<ICardArticleQueue, CardArticleQueue>();
            services.AddTransient<ISemanticCardArticleQueue, SemanticCardArticleQueue>();

            services.AddTransient<IBanlistHtmlDocument, BanlistHtmlDocument>();
            services.AddTransient<IBanlistWebPage, BanlistWebPage>();
            services.AddTransient<IHtmlWebPage, HtmlWebPage>();

            return services;
        }
    }
}
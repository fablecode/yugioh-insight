using article.core.Models;
using article.domain.Services;
using article.domain.Services.Messaging;
using article.domain.Services.Messaging.Cards;
using article.domain.WebPages;
using article.domain.WebPages.Banlists;
using article.infrastructure.Services.Messaging;
using article.infrastructure.Services.Messaging.Cards;
using article.infrastructure.WebPages.Banlists;
using Microsoft.Extensions.DependencyInjection;

namespace article.infrastructure
{
    public static class InfrastructureInstaller
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services
                .AddMessagingServices()
                .AddHtmlServices();

            return services;
        }
        public static IServiceCollection AddMessagingServices(this IServiceCollection services)
        {
            services.AddTransient<IQueue<Article>, ArticleQueue>();
            services.AddTransient<IBanlistArticleQueue, BanlistArticleQueue>();

            services.AddTransient<ICardArticleQueue, CardArticleQueue>();
            services.AddTransient<ISemanticCardArticleQueue, SemanticCardArticleQueue>();

            return services;
        }

        public static IServiceCollection AddHtmlServices(this IServiceCollection services)
        {
            services.AddTransient<IBanlistHtmlDocument, BanlistHtmlDocument>();
            services.AddTransient<IBanlistWebPage, BanlistWebPage>();
            services.AddTransient<IHtmlWebPage, HtmlWebPage>();

            return services;
        }
    }
}